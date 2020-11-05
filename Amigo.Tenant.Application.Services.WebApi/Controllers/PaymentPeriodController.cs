using Amigo.Tenant.Application.DTOs.FileRepository.Requests;
using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;
using Amigo.Tenant.Application.Services.Interfaces.FileRepository;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.PaymentPeriod;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.PaymentPeriod;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/payment")]//,CachingMasterData]
    public class PaymentPeriodController : ApiController
    {
        private readonly IPaymentPeriodApplicationService _paymentPeriodApplicationService;
        private readonly IConceptApplicationService _conceptApplicationService;
        private readonly IFileRepositoryApplicationService _fileRepositoryAppService;
        private readonly IMapper _mapper;


        public PaymentPeriodController(IPaymentPeriodApplicationService paymentPeriodApplicationService, 
                IConceptApplicationService conceptApplicationService,
                IFileRepositoryApplicationService fileRepositoryAppService,
                IMapper mapper)
        {
            _paymentPeriodApplicationService = paymentPeriodApplicationService;
            _conceptApplicationService = conceptApplicationService;
            _fileRepositoryAppService = fileRepositoryAppService;
            _mapper = mapper;
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

        [HttpGet, Route("searchForLiquidation")]
        public async Task<ResponseDTO<PPHeaderSearchByContractPeriodDTO>> SearchForLiquidation([FromUri]PaymentPeriodSearchByContractPeriodRequest search)
        {
            var resp = await _paymentPeriodApplicationService.SearchForLiquidation(search);
            return resp;
        }

        [HttpGet, Route("sendEmailAboutLiquidation")]
        public async Task<ResponseDTO<PPHeaderSearchByContractPeriodDTO>> SendEmailAboutLiquidation([FromUri]PaymentPeriodSearchByContractPeriodRequest search)
        {
            var resp = await _paymentPeriodApplicationService.SearchForLiquidation(search);
            var invoiceList = _mapper.Map<List<PPDetailSearchByContractPeriodDTO>, List<PPHeaderSearchByInvoiceDTO>>(resp.Data.PPDetail);
            CompleteInformation(invoiceList, resp.Data);
            await GenerateFileAndSend(invoiceList, true);
            return resp;
        }

        private void CompleteInformation(List<PPHeaderSearchByInvoiceDTO> invoiceList, PPHeaderSearchByContractPeriodDTO pPHeader)
        {
            foreach (var item in invoiceList)
            {
                item.TenantFullName = pPHeader.TenantFullName;
                item.UserName = pPHeader.Username;
                item.HouseName = pPHeader.HouseName;
                item.Email = pPHeader.Email;
            }
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
                try
                {
                    var response = await _paymentPeriodApplicationService.UpdatePaymentPeriodAsync(paymentPeriod);
                    
                    if (response.IsValid )
                    {
                        var invoiceId = ((RegisteredCommandResult)((ResponseDTO<CommandResult>)response).Data).Id;
                        if (invoiceId > 0)
                        {
                            try
                            {
                                var resp = await _paymentPeriodApplicationService.SearchInvoiceByIdAsync("", invoiceId);
                                await GenerateFileAndSend(resp.Data);
                                return response;
                            }
                            catch (Exception ex)
                            {
                                return CreateErrorResponse(ex, "Error intentando adjuntar el archivo");
                            }
                        }
                        else
                        {
                            throw new Exception("Error intentando extraer el invoice Id del payment registrado");
                        }
                    }
                    else{
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return CreateErrorResponse(ex, "Error intentando actualizar el pago");
                }
                
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.PaymentPeriodUpdate)]
        public async Task<ResponseDTO> Delete([FromBody] PPDeleteDTO paymentPeriod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _paymentPeriodApplicationService.DeletePaymentPeriodAsync(paymentPeriod);
                    return response;
                }
                catch (Exception ex)
                {
                    return CreateErrorResponse(ex, "Error intentando eliminar los Pagos");
                }

            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("massDelete")] 
        public async Task<ResponseDTO> MassDelete([FromBody] List<PPDeleteDTO> request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _paymentPeriodApplicationService.MassDeletePaymentPeriodAsync(request);
                    return response;
                }
                catch (Exception ex)
                {
                    return CreateErrorResponse(ex, "Error intentando eliminar los Pagos");
                }

            }
            return ModelState.ToResponse();
        }

        private ResponseDTO CreateErrorResponse(Exception ex, string errorPrefixMessage)
        {
            var errorMessage = ex.Message;
            var stackTrace = ex.StackTrace.ToString();

            var response = new ResponseDTO()
            {
                IsValid = false,
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = "Error",
                Message = string.Format("{0} {1} {2} {3}", errorPrefixMessage, errorMessage, Environment.NewLine, stackTrace)
            });
            return response;
        }

        private async Task GenerateFileAndSend(List<PPHeaderSearchByInvoiceDTO> paymentPeriodList, bool? isPreLiquidation=false)
        {

            var paymentPeriod = paymentPeriodList.First();

            //Nombre Archivo
            var fileName = string.Format("Invoice_{0}_{1}_{2}.pdf", paymentPeriod.InvoiceNo, paymentPeriod.PeriodCode, DateTime.Now);

            //Generar Documento
            MemoryStream memStream = CrearDocumentoAmigo(paymentPeriodList, isPreLiquidation);

            //Grabar en File Repository
            if (!isPreLiquidation.Value)
            {
                await SaveInFileRepositoryAsync(memStream, paymentPeriod, fileName);
            }

            //Crear Archivo Adjunto
            var attachmentList = new List<Attachment>();

            var attachment = new Attachment(new MemoryStream(memStream.ToArray()), System.Net.Mime.MediaTypeNames.Application.Pdf);
            attachment.ContentDisposition.FileName = fileName;
            attachmentList.Add(attachment);

            //Recuperar Archivos Adjuntos
            var paymentPeriodListIds = paymentPeriodList.Select(q => q.PaymentPeriodId.Value).Distinct().ToList();
            var fileRepositoryList = await _fileRepositoryAppService.GetFileRepositoriesByIdListAsync(Constants.EntityCode.Payment, paymentPeriodListIds);
            foreach (var item in fileRepositoryList.Data.Items)
            {
                var utMediaFile = await _fileRepositoryAppService.GetFileRepositoryByIdAsync(item.FileRepositoryId);
                if (utMediaFile != null)
                {
                    var attach = new Attachment(new MemoryStream(utMediaFile.UtMediaFile), item.ContentType);
                    attach.ContentDisposition.FileName = item.Name + item.FileExtension;
                    attachmentList.Add(attach);
                }
            }

            //Enviar Correo
            await SendEmail(attachmentList, paymentPeriod);

        }

        private async Task SaveInFileRepositoryAsync(MemoryStream memStream, PPHeaderSearchByInvoiceDTO paymentPeriod, string fileName)
        {
            FileRepositoryRequest request = new FileRepositoryRequest()
            {
                EntityCode = Constants.EntityCode.Invoice,
                ParentId = paymentPeriod.InvoiceId,
                AdditionalInfo = fileName
            };

            ContentType contentType = new ContentType("application/pdf");
            var fileExtension = "pdf";
            byte[] bytes = memStream.ToArray();
            var entityDtoRequest = new FileRepositoryEntityDTO()
            {
                Name = fileName,
                ParentId = request.ParentId.Value,
                UtMediaFile = bytes,
                EntityCode = request.EntityCode,
                ContentType = "application/pdf",
                FileExtension = fileExtension,
                AdditionalInfo = request.AdditionalInfo
            };
            await _fileRepositoryAppService.RegisterAsync(entityDtoRequest);
        }

        private async Task SendEmail(List<Attachment> attachmentList, PPHeaderSearchByInvoiceDTO headerPayment, bool? isPreLiquidation = false)
        {
            MailConfiguration mailConfig = new MailConfiguration();
            var fromEmail = System.Configuration.ConfigurationManager.AppSettings["fromEmail"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            var password = System.Configuration.ConfigurationManager.AppSettings["password"];
            var isTenantEmailEnabled = System.Configuration.ConfigurationManager.AppSettings["tenantEmailEnabled"];


            StringBuilder body = new StringBuilder("<!DOCTYPE html><html><head><meta charset='UTF-8'></head>");
            body.AppendLine("<body>");
            if (isPreLiquidation.Value)
            {
                body.AppendLine("<h3>Notificación x PreLiquidacion: </h3><br/>");
            }
            else
            {     
                body.AppendLine(string.Format("<h3>Notificación de Pago: Invoice Nro {0}</h3><br/>", headerPayment.InvoiceNo));
                body.AppendLine(string.Format("<p>Estimado <b>{0}</b>, la presente es para notificar que el dia {1} Ud. realizó un pago, por la propiedad '<b><i>{2}</i></b>'. Adjuntamos recibo de pago.</p><br/>", headerPayment.TenantFullName, headerPayment.InvoiceDate.Value.ToShortDateString(), headerPayment.HouseName));
            }
            body.AppendLine("<p>Atentamente,</p><br/>");
            body.AppendLine("<p><b>LA GERENCIA</b></p><br/>");
            body.AppendLine("</body></html>");

            var emailBody = body.ToString();
            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = isPreLiquidation.Value ? "Preliquidacion" : string.Format("Amigo Tenant Invoice Nro. {0}", headerPayment.InvoiceNo),
                Body = emailBody,
                IsBodyHtml = true
            };
            //mail.Attachments .Add(attachmentList);
            attachmentList.ForEach(q => {
                mail.Attachments.Add(q);
            });

            if (bool.Parse(isTenantEmailEnabled)) { 
                mail.To.Add(headerPayment.Email);
            } else
            {
                mail.To.Add(fromEmail);
            }

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(fromEmail, password),
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
            doc.AddCreator("Paul Romero");

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

        private MemoryStream CrearDocumentoAmigo(List<PPHeaderSearchByInvoiceDTO> paymentPeriod, bool? isPreLiquidation = false)
        {
            var isLiquidating = paymentPeriod.Select(q => q.PeriodCode).Distinct().Count() > 1;

            Document doc = new Document(PageSize.LETTER);
            MemoryStream memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memStream);

            doc.AddTitle("Invoice for Amigo Tenant Copyright");
            doc.AddCreator("Paul Romero");

            // Abrimos el archivo
            doc.Open();

            //// Escribimos el encabezamiento en el documento
            doc.Add(Chunk.NEWLINE);

            //string imagepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/logo_invoice.png"); //Application.StartupPath;
            Image jpg = Image.GetInstance(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/logo_invoice.png"));
            jpg.ScaleToFit(100f, 50f);
            jpg.SpacingAfter = 12f;
            jpg.SpacingBefore = 12f;
            
            doc.Add(jpg);

            iTextSharp.text.Font _standardFontTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
            var paragraph = new Paragraph(isPreLiquidation.Value?"P R E   L I Q U I D A T I O N":"I N V O I C E", _standardFontTitle);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);
            doc.Add(Chunk.NEWLINE);

            //SELLO DE AGUA
            PdfContentByte contentByte = writer.DirectContentUnder;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1257, BaseFont.NOT_EMBEDDED);
            contentByte.SetColorFill(BaseColor.LIGHT_GRAY);
            contentByte.SetFontAndSize(bf, 68);
            contentByte.BeginText();
            Rectangle pageSize = doc.PageSize;
            var x = (pageSize.Right + pageSize.Left) / 2;
            var y = ((pageSize.Bottom + pageSize.Top) / 2)+ 80;
            string text = isPreLiquidation.Value?"D  R  A  F  T":Constants.EntityStatus.InvoiceName.Paid;
            contentByte.ShowTextAligned(Element.ALIGN_MIDDLE, text, x, y, 40);
            contentByte.EndText();

            // Creamos una tabla que contendrá el nombre, apellido y país 
            var firstPaymentPeriod = paymentPeriod.First();
            var periodCode = firstPaymentPeriod.PeriodCode;
            if (isLiquidating)
            {
                periodCode = paymentPeriod.FirstOrDefault(q => q.ConceptCode == Constants.ConceptCode.DepositDevol).PeriodCode;
            }
            
            CreateHeaderInvoice(doc, firstPaymentPeriod, periodCode, isPreLiquidation);
            doc.Add(Chunk.NEWLINE);

            CreateDetailInvoice(doc, paymentPeriod, isPreLiquidation);
            doc.Add(Chunk.NEWLINE);

            CreateFooterInvoice(doc, paymentPeriod, isLiquidating, isPreLiquidation );
            doc.Add(Chunk.NEWLINE);

            doc.Close();
            writer.Close();
            return memStream;
        }

        private void CreateFooterInvoice(Document doc, List<PPHeaderSearchByInvoiceDTO> invoiceDataList, bool isLiquidating, bool? isPreLiquidation = false)
        {
            var invoiceData = invoiceDataList.First();
            Font _standardFontSubTitle = new Font(Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            Font _standardFontSubTitleData = new Font(Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            PdfPTable tblHeader = new PdfPTable(3);
            tblHeader.WidthPercentage = 100;
            tblHeader.DefaultCell.Border = Rectangle.NO_BORDER;

            //TITULOS

            PdfPCell clSubTitleSeparation = new PdfPCell();
            clSubTitleSeparation.BorderWidth = 0;

            PdfPCell clSubTitleAmount = new PdfPCell(new Phrase("Total Amount", _standardFontSubTitle));
            clSubTitleAmount.BorderWidth = 0;
            clSubTitleAmount.HorizontalAlignment = 1;

            PdfPCell clSubTitleBalance = new PdfPCell(new Phrase("Balance", _standardFontSubTitle));
            clSubTitleBalance.BorderWidth = 0;
            clSubTitleBalance.HorizontalAlignment = 1;

            //DATOS

           

            decimal? balance = 0;
            decimal? totalAmount = 0;
            if (!isLiquidating && !isPreLiquidation.Value) {
                balance = invoiceData.TotalIncome - invoiceData.TotalInvoice;
                totalAmount = invoiceData.TotalAmount;
            }
            else
            {
                totalAmount = invoiceDataList.Sum(q => q.PaymentAmount);
            }

            PdfPCell clSubTitleAmountData = new PdfPCell(new Phrase(totalAmount.ToString(), _standardFontSubTitleData));
            clSubTitleAmountData.BorderWidth = 0;
            clSubTitleAmountData.HorizontalAlignment = 1;

            PdfPCell clSubTitleBalanceData = new PdfPCell(new Phrase(balance.ToString(), _standardFontSubTitleData));
            clSubTitleBalanceData.BorderWidth = 0;
            clSubTitleBalanceData.HorizontalAlignment = 1;

            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleAmount);
            tblHeader.AddCell(clSubTitleAmountData);

            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleBalance);
            tblHeader.AddCell(clSubTitleBalanceData);

            doc.Add(tblHeader);
        }

        private void CreateDetailInvoice(Document doc, List<PPHeaderSearchByInvoiceDTO> paymentPeriod, bool? isPreLiquidation = false)
        {

            iTextSharp.text.Font _standardFontHeader = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font _standardFontBody = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // de nuestros visitante.
            PdfPTable tblDetail = new PdfPTable(3);
            tblDetail.WidthPercentage = 100;
            tblDetail.DefaultCell.Border = Rectangle.NO_BORDER;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clSequence = new PdfPCell(new Phrase("No.", _standardFontHeader));
            clSequence.BorderWidth = 0;
            clSequence.BorderWidthBottom = 0.75f;

            PdfPCell clDescription = new PdfPCell(new Phrase("Description", _standardFontHeader));
            clDescription.BorderWidth = 0;
            clDescription.BorderWidthBottom = 0.75f;

            PdfPCell clAmount = new PdfPCell(new Phrase("Amount", _standardFontHeader));
            clAmount.BorderWidth = 0;
            clAmount.BorderWidthBottom = 0.75f;
            clAmount.HorizontalAlignment = 1;

            // Añadimos las celdas a la tabla
            tblDetail.AddCell(clSequence);
            tblDetail.AddCell(clDescription);
            tblDetail.AddCell(clAmount);

            // Llenamos la tabla con información
            var sequence = 1;
            paymentPeriod.ForEach(q =>
            {
                clSequence = new PdfPCell(new Phrase(sequence.ToString(), _standardFontBody));
                clSequence.BorderWidth = 0;

                clDescription = new PdfPCell(new Phrase(q.PaymentDescription, _standardFontBody));
                clDescription.BorderWidth = 0;

                clAmount = new PdfPCell(new Phrase(q.PaymentAmount.ToString(), _standardFontBody));
                clAmount.BorderWidth = 0;
                clAmount.HorizontalAlignment = 1;

                // Añadimos las celdas a la tabla
                tblDetail.AddCell(clSequence);
                tblDetail.AddCell(clDescription);
                tblDetail.AddCell(clAmount);

                sequence++;
            });

            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            doc.Add(tblDetail);
        }

        private void CreateHeaderInvoice(Document doc, PPHeaderSearchByInvoiceDTO paymentPeriod, string periodCode, bool? isPreLiquidation = false)
        {
            iTextSharp.text.Font _standardFontSubTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font _standardFontSubTitleData = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            PdfPTable tblHeader = new PdfPTable(5);
            tblHeader.WidthPercentage = 100;
            tblHeader.DefaultCell.Border = Rectangle.NO_BORDER;

            // Configuramos el TITULOS de las columnas de la tabla
            PdfPCell clSubTitleSeparation = new PdfPCell();
            clSubTitleSeparation.BorderWidth = 0;

            PdfPCell clSubTitleInvoice = new PdfPCell(new Phrase("Invoice No.", _standardFontSubTitle));
            clSubTitleInvoice.BorderWidth = 0;

            PdfPCell clSubTitlePeriod = new PdfPCell(new Phrase("Period", _standardFontSubTitle));
            clSubTitlePeriod.BorderWidth = 0;

            PdfPCell clSubTitleDate = new PdfPCell(new Phrase("Date", _standardFontSubTitle));
            clSubTitleDate.BorderWidth = 0;

            PdfPCell clSubTitleUser = new PdfPCell(new Phrase("User", _standardFontSubTitle));
            clSubTitleUser.BorderWidth = 0;

            PdfPCell clSubTitleBillTo = new PdfPCell(new Phrase("Bill To", _standardFontSubTitle));
            clSubTitleBillTo.BorderWidth = 0;

            PdfPCell clSubTitleAddress = new PdfPCell(new Phrase("Address", _standardFontSubTitle));
            clSubTitleAddress.BorderWidth = 0;

            PdfPCell clSubTitleComment = new PdfPCell(new Phrase("Comment", _standardFontSubTitle));
            clSubTitleComment.BorderWidth = 0;

            PdfPCell clSubTitleAddressAmigo = new PdfPCell(new Phrase("", _standardFontSubTitle));
            clSubTitleAddressAmigo.BorderWidth = 0;

            //Configracipon de DATOS del header
            PdfPCell clSubTitleInvoiceData = new PdfPCell(new Phrase(paymentPeriod.InvoiceNo, _standardFontSubTitleData));
            clSubTitleInvoiceData.BorderWidth = 0;

            PdfPCell clSubTitlePeriodData = new PdfPCell(new Phrase(periodCode, _standardFontSubTitleData));
            clSubTitlePeriodData.BorderWidth = 0;

            PdfPCell clSubTitleDateData = new PdfPCell(new Phrase(DateTime.Today.ToShortDateString(), _standardFontSubTitleData));
            clSubTitleDateData.BorderWidth = 0;

            PdfPCell clSubTitleUserData = new PdfPCell(new Phrase(paymentPeriod.UserName, _standardFontSubTitleData));
            clSubTitleUserData.BorderWidth = 0;

            PdfPCell clSubTitleBillToData = new PdfPCell(new Phrase(paymentPeriod.TenantFullName, _standardFontSubTitleData));
            clSubTitleBillToData.BorderWidth = 0;

            PdfPCell clSubTitleAddressData = new PdfPCell(new Phrase(paymentPeriod.HouseName, _standardFontSubTitleData));
            clSubTitleAddressData.BorderWidth = 0;

            PdfPCell clSubTitleCommentData = new PdfPCell(new Phrase(paymentPeriod.Comment, _standardFontSubTitleData));
            clSubTitleCommentData.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblHeader.AddCell(clSubTitleInvoice);
            tblHeader.AddCell(clSubTitleInvoiceData);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitlePeriod);
            tblHeader.AddCell(clSubTitlePeriodData);

            tblHeader.AddCell(clSubTitleDate);
            tblHeader.AddCell(clSubTitleDateData);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleUser);
            tblHeader.AddCell(clSubTitleUserData);

            tblHeader.AddCell(clSubTitleBillTo);
            tblHeader.AddCell(clSubTitleBillToData);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleAddress);
            tblHeader.AddCell(clSubTitleAddressData);

            tblHeader.AddCell(clSubTitleComment);
            tblHeader.AddCell(clSubTitleCommentData);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleSeparation);

            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleSeparation);
            tblHeader.AddCell(clSubTitleSeparation);


            doc.Add(tblHeader);
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

        [HttpPost, Route("deletePaymentDetail")]
        public async Task<ResponseDTO> DeletePaymentDetail([FromBody]PaymentPeriodUpdateRequest search)
        {
            var resp = await _paymentPeriodApplicationService.DeletePaymentPeriodDetailAsync(search);
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
        public async Task<HttpResponseMessage> PrintInvoiceById(int fileRepositoryId)
        {
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

            //var rowInicioDetalle = 16;
            //var rowTitulosDetalle = 15;

            //var resp = await _paymentPeriodApplicationService.SearchInvoiceByIdAsync(invoiceNo, null);
            //var c = 1;
            //var listFilter = from data in resp.Data
            //                 select new
            //                 {
            //                     Sequence = c++,
            //                     PaymentType = data.PaymentTypeCode,
            //                     Description = data.PaymentDescription,
            //                     Amount = data.PaymentAmount
            //                 };

            //try
            //{
            //    var ruta = System.Web.Hosting.HostingEnvironment.MapPath("~/AmigoInvoice.xlsx");

            //    var factory = ReportExportFactory.Create("EXCEL", ruta);

            //    var balance = resp.Data[0].TotalIncome - resp.Data[0].TotalInvoice;

            //    factory.SetPreHeader(
            //        new List<ReportHeader> {
            //            new ReportHeader() { Position = 6, PositionY = 2, Name =  resp.Data[0].InvoiceNo },
            //            new ReportHeader() { Position = 6, PositionY = 4, Name =  resp.Data[0].PeriodCode },
            //            new ReportHeader() { Position = 7, PositionY = 2, Name =  resp.Data[0].InvoiceDate.Value.ToShortDateString() },
            //            new ReportHeader() { Position = 7, PositionY = 4, Name =  resp.Data[0].UserName},
            //            new ReportHeader() { Position = 8, PositionY = 2, Name =  resp.Data[0].TenantFullName },
            //            new ReportHeader() { Position = 9, PositionY = 2, Name =  resp.Data[0].HouseName },
            //            new ReportHeader() { Position = 10, PositionY = 2, Name =  resp.Data[0].Comment },
            //            new ReportHeader() { Position = 26, PositionY = 4, Name =  resp.Data[0].TotalAmount.Value.ToString() },
            //            new ReportHeader() { Position = 29, PositionY = 4, Name =  balance.ToString() }
            //        });

            //    factory.SetHeader(
            //        new List<ReportHeader> {
            //            new ReportHeader() { Position = 1, Name =   "N°" },
            //            new ReportHeader() { Position = 2, Name =   "Payment Type" },
            //            new ReportHeader() { Position = 3, Name =   "Description"},
            //            new ReportHeader() { Position = 4, Name =   "Amount"},
            //        }, rowTitulosDetalle);

            //    factory.SetBody(listFilter.ToList());
            //    factory.Export("Invoice" + resp.Data[0].InvoiceNo + DateTime.Now.ToString("dd_MM_yy"), rowInicioDetalle);

            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Trace.TraceError($"DateTime: {DateTime.Now} searchCriteriaByInvoice Error: {ex.ToString()}");
            //    Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            //}

            //HttpResponseMessage rs = new HttpResponseMessage();
            //return rs;
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


        [HttpPost, Route("sendPaymentNotificationEmail")]
        public async Task<ResponseDTO> SendPaymentNotificationEMail([FromBody] List<PaymentPeriodSendNotificationListRequest> paymentPeriodContractIds)
        {
            //var resp = await _paymentPeriodApplicationService.SearchPaymentPeriodAsync(search);

            MailConfiguration mailConfig = new MailConfiguration();
            var fromEmail = System.Configuration.ConfigurationManager.AppSettings["fromEmail"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            var password = System.Configuration.ConfigurationManager.AppSettings["password"];
            var msg = new StringBuilder();
            

            foreach (var header in paymentPeriodContractIds)
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
