using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;
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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/payment")]//,CachingMasterData]
    public class PaymentPeriodController : ApiController
    {
        private readonly IPaymentPeriodApplicationService _paymentPeriodApplicationService;

        public PaymentPeriodController(IPaymentPeriodApplicationService paymentPeriodApplicationService)
        {
            _paymentPeriodApplicationService = paymentPeriodApplicationService;
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
                var response = await _paymentPeriodApplicationService.UpdatePaymentPeriodAsync(paymentPeriod);
                return response;
            }
            return ModelState.ToResponse();
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

        [AllowAnonymous]
        [HttpGet, Route("searchCriteriaByInvoice")]
        public async Task<HttpResponseMessage> PrintInvoiceById(string invoiceNo)
        {
            var resp = await _paymentPeriodApplicationService.SearchInvoiceByIdAsync(invoiceNo);

            var listFilter = from data in resp.Data
                             select new
                             {
                                 Sequence = data.PaymentTypeSequence,
                                 PaymentType = data.PaymentTypeCode,
                                 Description = data.PaymentDescription,
                                 Amount = data.PaymentAmount
                             };
            try
            {
                var ruta = System.Web.Hosting.HostingEnvironment.MapPath("~/AmigoInvoice.xlsx");

                var factory = ReportExportFactory.Create("EXCEL", ruta);

                factory.SetPreHeader(
                    new List<ReportHeader> {
                        new ReportHeader() { Position = 6, PositionY = 2, Name =  resp.Data[0].InvoiceNo },
                        new ReportHeader() { Position = 7, PositionY = 2, Name =  resp.Data[0].InvoiceDate.Value.ToShortDateString() },
                        new ReportHeader() { Position = 8, PositionY = 2, Name =  resp.Data[0].TenantFullName },
                        new ReportHeader() { Position = 9, PositionY = 2, Name =  resp.Data[0].HouseName },
                        new ReportHeader() { Position = 23, PositionY = 4, Name =  resp.Data[0].TotalAmount.Value.ToString() }
                    });

                factory.SetHeader(
                    new List<ReportHeader> {
                        new ReportHeader() { Position = 1, Name =   "N°" },
                        new ReportHeader() { Position = 2, Name =   "Payment Type" },
                        new ReportHeader() { Position = 3, Name =   "Description"},
                        new ReportHeader() { Position = 4, Name =   "Amount"},
                    }, 12);

                factory.SetBody(listFilter.ToList());
                factory.Export("Invoice" + resp.Data[0].InvoiceNo + DateTime.Now.ToString("dd_MM_yy"), 13);

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
                totalAmount += q.PaymentAmount;
                str.AppendLine(string.Format("<tr><td>{0}</td><td align='right'>{1}</td></tr>", q.PaymentDescription, string.Format("{0:N2}", q.PaymentAmount)));
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
