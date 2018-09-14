using Amigo.Tenant.Application.DTOs.Requests.Expense;
using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Expense;
using Amigo.Tenant.Application.Services.Interfaces.Expense;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/expense")]//,CachingMasterData]
    public class ExpenseController : ApiController
    {
        private readonly IExpenseApplicationService _expenseApplicationService;

        public ExpenseController(IExpenseApplicationService expenseApplicationService)
        {
            _expenseApplicationService = expenseApplicationService;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<ExpenseSearchDTO>>> Search([FromUri]ExpenseSearchRequest search)
        {
            var resp = await _expenseApplicationService.SearchExpenseAsync(search);
            return resp;            
        }

        [HttpGet, Route("getById")]
        public async Task<ResponseDTO<ExpenseRegisterRequest>> GetById(int? id)
        {
            var resp = await _expenseApplicationService.GetByIdAsync(id);
            return resp;
        }

        [HttpPost, Route("register") ]//, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ExpenseCreate)]
        public async Task<ResponseDTO> Register(ExpenseRegisterRequest expense)
        {
            if (ModelState.IsValid)
            {
                var response = await _expenseApplicationService.RegisterExpenseAsync(expense);
                return response;
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update") ] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ExpenseUpdate)]
        public async Task<ResponseDTO> Update(ExpenseUpdateRequest expense)
        {
            if (ModelState.IsValid)
            {
                return await _expenseApplicationService.UpdateExpenseAsync(expense);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete") ] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ExpenseDelete)]
        public async Task<ResponseDTO> Delete(ExpenseDeleteRequest expense)
        {
            if (ModelState.IsValid)
            {
                return await _expenseApplicationService.DeleteExpenseAsync(expense);
            }
            return ModelState.ToResponse();
        }

        /*EXCEL REPORT*/

        [HttpGet, Route("searchReport"), AllowAnonymous]//ShuttleClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public HttpResponseMessage SearchReport([FromUri]ExpenseSearchRequest search)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _expenseApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "expenseReportDTOs.csv",
                DispositionType = "inline"
            };
            return response;
        }


        [HttpPost, Route("changeStatus")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ExpenseUpdate)]
        public async Task<ResponseDTO> ChangeStatus(ExpenseChangeStatusRequest expense)
        {
            //TODO_: EL CHANGE STATUS SOLO DEBE PASAR EL CONTRACTID Y EL RESTO LLENARLO DESPUES
            if (ModelState.IsValid)
            {
                return await _expenseApplicationService.ChangeStatus(expense);
            }
            return ModelState.ToResponse();
        }


        /*DETAIL*/

        [HttpGet, Route("searchDetailCriteria")]
        public async Task<ResponseDTO<PagedList<ExpenseDetailSearchDTO>>> SearchDetail([FromUri]ExpenseSearchRequest search)
        {
            var resp = await _expenseApplicationService.GetDetailByExpenseIdAsync(search);
            return resp;
        }

        [HttpGet, Route("getExpenseDetailByExpenseId")]
        public async Task<ResponseDTO<ExpenseDetailRegisterRequest>> GetDetailByExpenseDetailIdAsync(int? id)
        {
            var resp = await _expenseApplicationService.GetDetailByExpenseDetailIdAsync(id);
            return resp;
        }

        [HttpPost, Route("registerDetail")]
        public async Task<ResponseDTO> RegisterDetail(ExpenseDetailRegisterRequest expense)
        {
            if (ModelState.IsValid)
            {
                var response = await _expenseApplicationService.RegisterExpenseDetailAsync(expense);
                return response;
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("updateDetail")] 
        public async Task<ResponseDTO> UpdateDetail(ExpenseDetailUpdateRequest expense)
        {
            if (ModelState.IsValid)
            {
                return await _expenseApplicationService.UpdateExpenseDetailAsync(expense);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("deleteDetail")]
        public async Task<ResponseDTO> DeleteDetail(ExpenseDetailDeleteRequest expense)
        {
            if (ModelState.IsValid)
            {
                return await _expenseApplicationService.DeleteExpenseDetailAsync(expense);
            }
            return ModelState.ToResponse();
        }

        [HttpGet, Route("searchDetailReport"), AllowAnonymous]
        public HttpResponseMessage SearchDetailReport([FromUri]ExpenseSearchRequest search)
        {
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _expenseApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "expenseReportDTOs.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [HttpPost, Route("changeDetailStatus")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.ExpenseDetailUpdate)]
        public async Task<ResponseDTO> ChangeDetailStatus(ExpenseDetailChangeStatusRequest expense)
        {
            //TODO_: EL CHANGE STATUS SOLO DEBE PASAR EL CONTRACTID Y EL RESTO LLENARLO DESPUES
            if (ModelState.IsValid)
            {
                return await _expenseApplicationService.ChangeDetailStatus(expense);
            }
            return ModelState.ToResponse();
        }

    }
}
