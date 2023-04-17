using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CustomizedServices
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public (int StatusCode, string Message, MailMessage email) ConfigurationEmailAsync(string to, string subject, string body)
        {
            MailMessage? email = new MailMessage();
            try
            {
                if (to == "" || subject == "" || body == "") return (StatusCodes.Status400BadRequest, "Require some data", email);
                var from = _config.GetSection("Smtp:From").Value;
                var host = _config.GetSection("Smtp:Host").Value;
                var port = _config.GetSection("Smtp:Port").Value;
                var password = _config.GetSection("Smtp:Password").Value;
                var displayName = _config.GetSection("Smtp:UserName").Value; ;

                email.From = new MailAddress(from, displayName);//Correo de salida
                email.To.Add(to); //Correo destino?
                email.Subject = subject; //Asunto
                email.Body = body; //Mensaje del correo
                email.IsBodyHtml = true;
                email.Priority = MailPriority.High;
                return (StatusCodes.Status200OK, "Successful", email);
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message, email);
            }
        }

        public (int StatusCode, string Message) SendEmailAsync(MailMessage email)
        {
            try
            {
                var from = _config.GetSection("Smtp:From").Value;
                var host = _config.GetSection("Smtp:Host").Value;
                var port = _config.GetSection("Smtp:Port").Value;
                var password = _config.GetSection("Smtp:Password").Value;

                SmtpClient client = new SmtpClient(host, int.Parse(port));
                client.Credentials = new NetworkCredential(from, password);//Cuenta de correo
                client.EnableSsl = true;//True si el servidor de correo permite ssl
                client.Send(email);
                return (StatusCodes.Status200OK, "Succesful send email");
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}