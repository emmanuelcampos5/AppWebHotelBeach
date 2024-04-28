using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;

namespace AppWebHotelBeach.Models
{
    public class Email
    {
        public void Enviar(Cliente cliente)
        {
            try
            {
                MailMessage email = new MailMessage();

                email.Subject = "Datos de registro en plataforma web del Hotel Beach.SA";

                email.To.Add(new MailAddress("lenguajes2023Hotel@outlook.com"));

                email.To.Add(new MailAddress(cliente.Email));

                email.From = new MailAddress("lenguajes2023Hotel@outlook.com");

                string html = "Bienvenidos a Hotel Beach.SA, gracias por preferirnos!";
                html += "<br> A continuacion detallamos sus datos registrados en nuestra base de Clientes";
                html += "<br><b>ID Login:</b>" + cliente.ID;
                html += "<br><b>Email:</b>" + cliente.Email;
                html += "<br><b>Su contraseña temporal:</b>" + cliente.Password;
                html += "<br><b>Este correo generado de forma automatica, favor no responderlo. ";
                html += "Por la plataforma web de Hotel Beach.SA.</b>";

                email.IsBodyHtml = true;

                email.Priority = MailPriority.Normal;

                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

                email.AlternateViews.Add(view);

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp-mail.outlook.com";

                smtp.Port = 587;

                smtp.EnableSsl = true;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("lenguajes2023Hotel@outlook.com", "Hotel123456");

                smtp.Send(email);

                email.Dispose();
                smtp.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
