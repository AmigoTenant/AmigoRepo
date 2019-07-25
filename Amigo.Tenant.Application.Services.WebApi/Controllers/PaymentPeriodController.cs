using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.PaymentPeriod;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Mail;
using Newtonsoft.Json;
using Nustache.Core;
using Report.Presentation.Tools;
using Report.Presentation.Tools.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/payment")]//,CachingMasterData]
    public class PaymentPeriodController : ApiController
    {
        private readonly IPaymentPeriodApplicationService _paymentPeriodApplicationService;
        private readonly IConceptApplicationService _conceptApplicationService;

        public PaymentPeriodController(IPaymentPeriodApplicationService paymentPeriodApplicationService, IConceptApplicationService conceptApplicationService)
        {
            _paymentPeriodApplicationService = paymentPeriodApplicationService;
            _conceptApplicationService = conceptApplicationService;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<PPSearchDTO>>> Search([FromUri]PaymentPeriodSearchRequest search)
        {
            var resp = await _paymentPeriodApplicationService.SearchPaymentPeriodAsync(search);
            return resp;
        }

        [HttpGet, Route("searchCriteriaByContract")]
        public async Task<ResponseDTO<PPHeaderSearchByContractPeriodDTO>> SearchCriteriaByContract([FromUri]PaymentPeriodSearchByContractPeriodRequest search)
        {
            var resp = await _paymentPeriodApplicationService.SearchPaymentPeriodByContractAsync(search);
            return resp;
        }

        [HttpPost]
        [Route("search")]
        public async Task<ResponseDTO<PagedList<PPSearchDTO>>> SearchServiceOrder([FromBody] PaymentPeriodSearchRequest search)
        {
            var resp = await _paymentPeriodApplicationService.SearchPaymentPeriodAsync(search);
            return resp;
        }


        [HttpPost, Route("update")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.PaymentPeriodUpdate)]
        public async Task<ResponseDTO> Update(PPHeaderSearchByContractPeriodDTO paymentPeriod)
        {
            if (ModelState.IsValid)
            {
                //var response = await _paymentPeriodApplicationService.UpdatePaymentPeriodAsync(paymentPeriod);
                await GenerateFileAndSend(paymentPeriod);
                return null; // response;
            }
            return ModelState.ToResponse();
        }

        private async Task GenerateFileAndSend(PPHeaderSearchByContractPeriodDTO paymentPeriod)
        {
            MemoryStream memStream = CrearDocumentoAmigo(paymentPeriod);
            var attachment = new Attachment(new MemoryStream(memStream.ToArray()), System.Net.Mime.MediaTypeNames.Application.Pdf);
            attachment.ContentDisposition.FileName = string.Format("Invoice_{0}.pdf", "Test");
            await SendEmail(attachment, paymentPeriod);
        }

        private async Task SendEmail(Attachment attachment, PPHeaderSearchByContractPeriodDTO paymentPeriod)
        {
            var emailBody = "<!DOCTYPE html><html><head><title>Título de la WEB</title><meta charset='UTF-8'></head><body><h1>TEST H1</h1><div><table><tr><td>celda1</td></tr></table></div></body></html>";
            var mail = new MailMessage
            {
                From = new MailAddress("pjromg@gmail.com"),
                Subject = string.Format("Payment Notification - Renta AmigoTenant - Periodo: {0}", "2019"),
                Body = emailBody,
                IsBodyHtml = true
            };
            mail.Attachments.Add(attachment);
            mail.To.Add("jamromguz@outlook.com");

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("pjromg@gmail.com", "jursaturnepplu"),
                EnableSsl = true
            };
            await client.SendMailAsync(mail);
        }

        private MemoryStream CrearDocumentoEjemplo()
        {
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);

            MemoryStream memStream = new MemoryStream();
            PdfWriter writer1 = PdfWriter.GetInstance(doc, memStream);

            // Indicamos donde vamos a guardar el documento
            //PdfWriter writer = PdfWriter.GetInstance(doc,
            //                            new FileStream(@"C:\prueba.pdf", FileMode.Create));

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Mi primer PDF");
            doc.AddCreator("Roberto Torres");

            // Abrimos el archivo
            doc.Open();

            // Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("Mi primer documento PDF"));
            doc.Add(Chunk.NEWLINE);

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Creamos una tabla que contendrá el nombre, apellido y país 
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(3);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clApellido = new PdfPCell(new Phrase("Apellido", _standardFont));
            clApellido.BorderWidth = 0;
            clApellido.BorderWidthBottom = 0.75f;

            PdfPCell clPais = new PdfPCell(new Phrase("País", _standardFont));
            clPais.BorderWidth = 0;
            clPais.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);

            // Llenamos la tabla con información
            clNombre = new PdfPCell(new Phrase("Roberto", _standardFont));
            clNombre.BorderWidth = 0;

            clApellido = new PdfPCell(new Phrase("Torres", _standardFont));
            clApellido.BorderWidth = 0;

            clPais = new PdfPCell(new Phrase("Puerto Rico", _standardFont));
            clPais.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);

            clNombre = new PdfPCell(new Phrase("Juan", _standardFont));
            clNombre.BorderWidth = 0;

            clApellido = new PdfPCell(new Phrase("Rodríguez", _standardFont));
            clApellido.BorderWidth = 0;

            clPais = new PdfPCell(new Phrase("México", _standardFont));
            clPais.BorderWidth = 0;

            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);
            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            doc.Add(tblPrueba);

            //intento1
            //memStream.Position = 0;


            doc.Close();
            writer1.Close();
            return memStream;
        }
        private MemoryStream CrearDocumentoAmigo(PPHeaderSearchByContractPeriodDTO paymentPeriod)
        {
            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);

            MemoryStream memStream = new MemoryStream();
            PdfWriter writer1 = PdfWriter.GetInstance(doc, memStream);
            //StringBuilder sb = CreateHtml();

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Mi primer PDF");
            doc.AddCreator("Roberto Torres");

            // Abrimos el archivo
            doc.Open();

            //// Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("Mi primer documento PDF"));
            doc.Add(Chunk.NEWLINE);

            //string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/logo_invoice.png"); //Application.StartupPath;
            Image jpg = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/logo_invoice.png"));
            jpg.ScaleToFit(300f, 300f);
            jpg.SpacingAfter = 12f;
            jpg.SpacingBefore = 12f;
            doc.Add(new Paragraph("GIF"));

            //Image gif = Image.GetInstance(imagepath + "/mikesdotnetting.gif");

            doc.Add(jpg);

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Creamos una tabla que contendrá el nombre, apellido y país 
            // de nuestros visitante.
            PdfPTable tblDetail = new PdfPTable(3);
            tblDetail.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clSequence = new PdfPCell(new Phrase("No.", _standardFont));
            clSequence.BorderWidth = 0;
            clSequence.BorderWidthBottom = 0.75f;

            PdfPCell clDescription = new PdfPCell(new Phrase("Description", _standardFont));
            clDescription.BorderWidth = 0;
            clDescription.BorderWidthBottom = 0.75f;

            PdfPCell clAmount = new PdfPCell(new Phrase("Amount", _standardFont));
            clAmount.BorderWidth = 0;
            clAmount.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblDetail.AddCell(clSequence);
            tblDetail.AddCell(clDescription);
            tblDetail.AddCell(clAmount);

            // Llenamos la tabla con información
            var sequence = 1;
            paymentPeriod.PPDetail.ForEach(q => 
            {
                clSequence = new PdfPCell(new Phrase(sequence.ToString(), _standardFont));
                clSequence.BorderWidth = 0;

                clDescription = new PdfPCell(new Phrase(q.PaymentDescription, _standardFont));
                clDescription.BorderWidth = 0;

                clAmount = new PdfPCell(new Phrase(q.PaymentAmount.ToString(), _standardFont));
                clAmount.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDetail.AddCell(clSequence);
                tblDetail.AddCell(clDescription);
                tblDetail.AddCell(clAmount);

                sequence++;
            });

            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            doc.Add(tblDetail);
            //memStream.Position = 0;


            doc.Close();
            writer1.Close();
            return memStream;
        }

        [HttpGet, Route("calculateLateFeeByContractAndPeriod")]
        public async Task<ResponseDTO<PPDetailSearchByContractPeriodDTO>> CalculateLateFeeByContractAndPeriod([FromUri]PaymentPeriodSearchByContractPeriodRequest search)
        {
            var resp = await _paymentPeriodApplicationService.CalculateLateFeeByContractAndPeriodAsync(search);
            return resp;
        }

        [HttpGet, Route("calculateOnAccountByContractAndPeriod")]
        public async Task<ResponseDTO<PPDetailSearchByContractPeriodDTO>> CalculateOnAccountByContractAndPeriod([FromUri]PaymentPeriodSearchByContractPeriodRequest search)
        {
            var resp = await _paymentPeriodApplicationService.CalculateOnAccountByContractAndPeriodAsync(search);
            return resp;
        }

        [HttpPost, Route("registerPaymentDetail")]
        public async Task<ResponseDTO> RegisterPaymentDetail([FromBody]PaymentPeriodRegisterRequest search)
        {
            var resp = await _paymentPeriodApplicationService.RegisterPaymentPeriodDetailAsync(search);
            return resp;
        }

        [HttpPost, Route("updatePaymentDetail")]
        public async Task<ResponseDTO> UpdatePaymentDetail([FromBody]PaymentPeriodUpdateRequest search)
        {
            var resp = await _paymentPeriodApplicationService.UpdatePaymentPeriodDetailAsync(search);
            return resp;
        }

        [HttpGet]
        [Route("exportToExcel/{periodId}/{houseId}/{contractCode}/{paymentPeriodStatusId}/{tenantId}/{hasPendingServices}/{hasPendingFines}/{hasPendingLateFee}/{page}/{pageSize}"), AllowAnonymous]//ShuttleClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.WeeklyReportSearch)
        public async Task<HttpResponseMessage> ExportToExcel([FromUri]PaymentPeriodSearchRequest search)
        {
            var response = Request.CreateResponse();
            search.ContractCode = null;
            response.Content = new PushStreamContent((outputStream, httpContent, transportContext)
                => _paymentPeriodApplicationService.GenerateDataCsvToReportExcel(outputStream, httpContent, transportContext, search), new MediaTypeHeaderValue("text/csv"));
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "paymentPeriod.csv",
                DispositionType = "inline"
            };
            return response;
        }

        [AllowAnonymous]
        [HttpGet, Route("searchCriteriaByInvoice")]
        public async Task<HttpResponseMessage> PrintInvoiceById(string invoiceNo)
        {
            var rowInicioDetalle = 16;
            var rowTitulosDetalle = 15;

            var resp = await _paymentPeriodApplicationService.SearchInvoiceByIdAsync(invoiceNo);
            var c = 1;
            var listFilter = from data in resp.Data
                             select new
                             {
                                 Sequence = c++,
                                 PaymentType = data.PaymentTypeCode,
                                 Description = data.PaymentDescription,
                                 Amount = data.PaymentAmount
                             };

            try
            {
                var ruta = System.Web.Hosting.HostingEnvironment.MapPath("~/AmigoInvoice.xlsx");

                var factory = ReportExportFactory.Create("EXCEL", ruta);

                var balance = resp.Data[0].TotalIncome - resp.Data[0].TotalInvoice;

                factory.SetPreHeader(
                    new List<ReportHeader> {
                        new ReportHeader() { Position = 6, PositionY = 2, Name =  resp.Data[0].InvoiceNo },
                        new ReportHeader() { Position = 6, PositionY = 4, Name =  resp.Data[0].PeriodCode },
                        new ReportHeader() { Position = 7, PositionY = 2, Name =  resp.Data[0].InvoiceDate.Value.ToShortDateString() },
                        new ReportHeader() { Position = 7, PositionY = 4, Name =  resp.Data[0].UserName},
                        new ReportHeader() { Position = 8, PositionY = 2, Name =  resp.Data[0].TenantFullName },
                        new ReportHeader() { Position = 9, PositionY = 2, Name =  resp.Data[0].HouseName },
                        new ReportHeader() { Position = 10, PositionY = 2, Name =  resp.Data[0].Comment },
                        new ReportHeader() { Position = 26, PositionY = 4, Name =  resp.Data[0].TotalAmount.Value.ToString() },
                        new ReportHeader() { Position = 29, PositionY = 4, Name =  balance.ToString() }
                    });

                factory.SetHeader(
                    new List<ReportHeader> {
                        new ReportHeader() { Position = 1, Name =   "N°" },
                        new ReportHeader() { Position = 2, Name =   "Payment Type" },
                        new ReportHeader() { Position = 3, Name =   "Description"},
                        new ReportHeader() { Position = 4, Name =   "Amount"},
                    }, rowTitulosDetalle);

                factory.SetBody(listFilter.ToList());
                factory.Export("Invoice" + resp.Data[0].InvoiceNo + DateTime.Now.ToString("dd_MM_yy"), rowInicioDetalle);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"DateTime: {DateTime.Now} searchCriteriaByInvoice Error: {ex.ToString()}");
                Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            HttpResponseMessage rs = new HttpResponseMessage();
            return rs;
        }

        private List<ResultOperationAuthorizedDTO> GetDispatchedNotPaidOrReceived()
        {
            var result = new List<ResultOperationAuthorizedDTO>();
            var dto = new ResultOperationAuthorizedDTO();
            dto.ActionType = "ActionType";
            dto.OperationAuthorizedExempt = "OperationAuthorizedExempt";
            dto.DocumentType = "DocumentType";
            dto.DocumentNo = "DocumentNo";
            dto.Carrier = "Carrier";
            dto.EquipmentNo = "EquipmentNo";
            dto.Quantity = 100;
            dto.EquipmentSizeType = "20";
            dto.AuthorizationNo = "AuthorizationNo";
            dto.InvoiceNumber = "InvoiceNumber";
            dto.CustomerIdentification = "CustomerIdentification";
            dto.Customer = "Customer";
            dto.VesselName = "VesselName";
            dto.VoyageNumber = "VoyageNumber";
            dto.YardOrigin = "YardOrigin";
            dto.YardDestination = "YardDestination";
            dto.Dispatched = "Dispatched";
            dto.ExternalUser = "ExternalUser";
            //dto.EquipmentActivityDate = DateTime.Now;
            //dto.EquipmentActivityTime = DateTime.Now.ToLocalTime().ToString("HH:mm");
            //dto.AutorizationDate = DateTime.Now;
            //dto.AutorizationTime = DateTime.Now.ToLocalTime().ToString("HH:mm");
            result.Add(dto);
            result.Add(dto);
            result.Add(dto);
            result.Add(dto);
            return result.ToList();
        }


        [HttpGet, Route("sendPaymentNotificationEmail")]
        public async Task<ResponseDTO> SendPaymentNotificationEMail([FromUri] PaymentPeriodSearchRequest search)
        {
            var resp = await _paymentPeriodApplicationService.SearchPaymentPeriodAsync(search);

            MailConfiguration mailConfig = new MailConfiguration();
            var fromEmail = System.Configuration.ConfigurationManager.AppSettings["fromEmail"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            var password = System.Configuration.ConfigurationManager.AppSettings["password"];
            var msg = new StringBuilder();
            

            foreach (var header in resp.Data.Items.ToList())
            {
                var searchByContractAndPeriod = new PaymentPeriodSearchByContractPeriodRequest();
                searchByContractAndPeriod.ContractId = header.ContractId;
                searchByContractAndPeriod.PeriodId = header.PeriodId;

                var paymentPeriod = await _paymentPeriodApplicationService.SearchPaymentPeriodByContractAsync(searchByContractAndPeriod);
                var template = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Templates/PayNotificationTemplate.html"));

                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(paymentPeriod.Data));
                var paymentDetailHtml = string.Empty;
                if (paymentPeriod.Data.PPDetail.Any())
                {
                    try
                    {

                        if (paymentPeriod.Data.Email == null)
                        {
                            msg.AppendLine(string.Format("Email for: {0} is null {1}", paymentPeriod.Data.TenantFullName, Environment.NewLine));
                            continue;
                        }

                        var toEmail = paymentPeriod.Data.Email;
                        paymentDetailHtml = await CreatePaymentDetailHtml(paymentPeriod.Data.PPDetail);
                        data.Add("PaymentPeriodDetail", paymentDetailHtml);
                        var emailBody = ProcessTemplate(template, data);
                        var mail = new MailMessage
                        {
                            From = new MailAddress(fromEmail),
                            Subject = string.Format("Payment Notification - Renta AmigoTenant - Periodo: {0}", header.PeriodCode),
                            Body = emailBody,
                            IsBodyHtml = true
                        };

                        mail.To.Add(toEmail);

                        var client = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new System.Net.NetworkCredential(userName, password),
                            EnableSsl = true
                        };

                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        msg.AppendLine(string.Format("ERROR trying to send email for: {0}{1}{2}", paymentPeriod.Data.TenantFullName, Environment.NewLine, ex.StackTrace.ToString()));
                    }
                    
                }
            }

            var response = new ResponseDTO()
            {
                IsValid = string.IsNullOrEmpty(msg.ToString()),
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = string.IsNullOrEmpty(msg.ToString()) ? "Ok" : "Error",
                Message = msg.ToString()
            });

            return response;
        }

        private async Task<string> CreatePaymentDetailHtml(List<PPDetailSearchByContractPeriodDTO> pPDetail)
        {
            decimal? totalAmount = 0;
            StringBuilder str = new StringBuilder();
            str.AppendLine("<table border = '1'><tr><th>Descripcion</th><th>Monto</th></tr>");
            pPDetail.ForEach(q => {
                if (!q.IsTenantFavorable.Value)
                {
                    totalAmount += q.PaymentAmount;
                    str.AppendLine(string.Format("<tr><td>{0}</td><td align='right'>{1}</td></tr>", q.PaymentDescription, string.Format("{0:N2}", q.PaymentAmount)));
                }
                else
                {
                    totalAmount -= q.PaymentAmount;
                    str.AppendLine(string.Format("<tr><td>{0}</td><td align='right' style='color: blue'>({1})</td></tr>", q.PaymentDescription, string.Format("{0:N2}", q.PaymentAmount)));
                }

                
            });
            str.AppendLine(string.Format("<tr><td><b>Total</b></td><td align='right'><b>{0}</b></td></tr>", string.Format("{0:N2}", totalAmount)));
            str.AppendLine("</table>");
            return str.ToString();
        }

        private static string ProcessTemplate(string template,
            Dictionary<string, object> data)
        {
            //return Regex.Replace(template, "\\{\\{(.*?)\\}\\}", m =>
            //    m.Groups.Count > 1 && data.ContainsKey(m.Groups[1].Value) ?
            //        //data[m.Groups[1].Value] : m.Value);
            return Render.StringToString(template, data);
        }

    }

    public class ResultOperationAuthorizedDTO
    {
        public string ActionType { get; set; }
        public string OperationAuthorizedExempt { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string Carrier { get; set; }
        public string EquipmentNo { get; set; }
        public int Quantity { get; set; }
        public string EquipmentSizeType { get; set; }
        public string AuthorizationNo { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerIdentification { get; set; }
        public string Customer { get; set; }
        public string VesselName { get; set; }
        public string VoyageNumber { get; set; }
        public string YardOrigin { get; set; }
        public string YardDestination { get; set; }
        public bool isDispatched { get; set; }
        public string ExternalUser { get; set; }
        public string Dispatched { get; set; }
        public DateTimeOffset AuthorizationDate { get; set; }
        public DateTimeOffset? ActivityDate { get; set; }
    }

    public class SearchDispatchedNotPaidOrReceivedDTO
    {
        public string RefereceDocumentNo { get; set; }
        public int LanguageId { get; set; }

        public DateTimeOffset? BeginDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public DateTimeOffset? FromGate { get; set; }
        public DateTimeOffset? ToGate { get; set; }

        public int? DocumentTypeId { get; set; }

        public int? OperationAuthorizedTypeId { get; set; }
    }
}
