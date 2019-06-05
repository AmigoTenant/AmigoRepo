using Amigo.Tenant.Application.DTOs.FileRepository.Requests;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using Amigo.Tenant.Application.Services.Interfaces.FileRepository;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Commands.FileRepository;
using Amigo.Tenant.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/filerepository")]
    public class FileRepositoryController : ApiController
    {
        private readonly IFileRepositoryApplicationService _fileRepositoryAppService;

        public FileRepositoryController(
            IFileRepositoryApplicationService fileRepositoryAppService)
        {
            _fileRepositoryAppService = fileRepositoryAppService;
        }

        [HttpGet]
        [Route("getFileRepositories/{entityCode}/{parentId}")]
        public async Task<ResponseDTO<PagedList<FileRepositoryDTO>>> GetFileRepositories(string entityCode, int? parentId)
        {
            var list = await _fileRepositoryAppService.GetFileRepositoriesAsync(entityCode, parentId);
            return list;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ResponseDTO> Upload()
        {
            var httpRequest = HttpContext.Current.Request;
            var postedImage = httpRequest.Files["File"];
            FileRepositoryRequest request = new FileRepositoryRequest()
            {
                EntityCode = httpRequest.Form["EntityCode"],
                ParentId = int.Parse(httpRequest.Form["ParentId"]),
                AdditionalInfo = httpRequest.Form["Additional"],
            };

            using (var mem = new MemoryStream())
            {
                var file = postedImage;
                if(file!=null)
                await file.InputStream.CopyToAsync(mem);

                var contentType = file?.ContentType;
                var fileName = file?.FileName.Split('.').First();
                var fileExtension = file?.FileName.Split('.').Last();
                byte[] bytes = file != null?mem.ToArray():null;
                var entityDtoRequest = new FileRepositoryEntityDTO()
                {
                    Name = fileName,
                    ParentId = request.ParentId.Value,
                    UtMediaFile = bytes,
                    EntityCode = request.EntityCode,
                    ContentType = contentType,
                    FileExtension = fileExtension,
                    AdditionalInfo = request.AdditionalInfo
                };
                return await _fileRepositoryAppService.RegisterAsync(entityDtoRequest);
            }
            return ModelState.ToResponse();
        }

        [HttpGet]
        [Route("download/{fileRepositoryId}")]
        public async Task<HttpResponseMessage> Download(int fileRepositoryId)
        {
            //Create HTTP Response.
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            var obj = await _fileRepositoryAppService.GetFileRepositoryByIdAsync(fileRepositoryId);
            var bytes = obj.UtMediaFile;

            //Set the Response Content.
            response.Content = new ByteArrayContent(bytes); 
            response.Content.Headers.ContentLength = bytes.LongLength; 

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = string.Format("{0}.{1}", obj.Name, obj.FileExtension),
            };
            response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(obj.ContentType);
            return response;
        }

        [HttpDelete]
        [Route("delete/{fileRepositoryId}")]
        public async Task<bool> Delete(int fileRepositoryId)
        {
            var wasDeleted = await _fileRepositoryAppService.DeleteAsync(fileRepositoryId);
            return wasDeleted;
        }

    }
}
