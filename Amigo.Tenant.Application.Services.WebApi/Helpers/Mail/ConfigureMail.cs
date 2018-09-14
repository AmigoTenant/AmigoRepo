using Newtonsoft.Json;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Amigo.Tenant.Mail
{
    public class MailConfiguration
    {

        public void ConfigureMail(byte[] bytesAttached)
        {
            //Helpers.Register("FullName", FullName);
            var fromEmail = System.Configuration.ConfigurationManager.AppSettings["fromEmail"];
            var toEmail = System.Configuration.ConfigurationManager.AppSettings["toEmail"];
            var userName = System.Configuration.ConfigurationManager.AppSettings["userName"];
            var password = System.Configuration.ConfigurationManager.AppSettings["password"];


            //=====================================
            //TO USE WITH TEMPLATE
            //=====================================

            //for (int i = 0; i < 50; i++)
            //{
                var template = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Templates/PayNotificationTemplate.html"));
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                    File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Templates/EmailData.json")));
                data.Add("date", DateTime.Now.ToShortDateString());

                var imageURL = System.Web.Hosting.HostingEnvironment.MapPath("~/logo.png");
                Image img = Image.FromFile(imageURL);
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    //return ms.ToArray();
                    data.Add("logoBytes", ms.ToArray());
                }

                var emailBody = ProcessTemplate(template, data);

                var mail = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Test Mail",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                //=====================================
                // From Phisical FILE
                //=====================================

                //Attachment adjunto = new Attachment(new MemoryStream(bytesAttached), "Invoice.pdf");

                //=====================================
                // From Phisical FILE
                //=====================================
                //string file = "data.xlsx"; //Este archivo debe existir en la raiz del compilado
                //Attachment adjunto = new Attachment(file, MediaTypeNames.Application.Octet);
                // Add time stamp information for the file.
                //ContentDisposition disposition = adjunto.ContentDisposition;
                //disposition.CreationDate = System.IO.File.GetCreationTime(file);
                //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

                // Add the file attachment to this e-mail message.
                //mail.Attachments.Add(adjunto);
                mail.To.Add(toEmail);

                var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(userName, password),
                    EnableSsl = true
                };

                client.Send(mail);

            //}

            
        }

        //private static void FullName(RenderContext context,
        //    IList<object> arguments, IDictionary<string, object> options,
        //    RenderBlock fn, RenderBlock inverse)
        //{
        //    if (arguments?.Count >= 2)
        //        context.Write($"{arguments[0]} {arguments[1]}");
        //}

        private static string ProcessTemplate(string template,
            Dictionary<string, object> data)
        {
            //return Regex.Replace(template, "\\{\\{(.*?)\\}\\}", m =>
            //    m.Groups.Count > 1 && data.ContainsKey(m.Groups[1].Value) ?
            //        //data[m.Groups[1].Value] : m.Value);
            return Render.StringToString(template, data);
        }
    }
}
