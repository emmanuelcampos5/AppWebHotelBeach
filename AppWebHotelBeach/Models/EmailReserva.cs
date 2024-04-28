using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using ApiHotelBeach.Models;

namespace AppWebHotelBeach.Models
{
    public class EmailReserva
    {
        

        public void Enviar(Reserva reserva, Paquete paquete, Cliente cliente)
        {
            try
            {                
                MailMessage email = new MailMessage();

                email.Subject = "Datos de registro en plataforma web CineMax CR";

                email.To.Add(new MailAddress("lenguajes2023Hotel@outlook.com"));

                email.To.Add(new MailAddress(cliente.Email));

                email.From = new MailAddress("lenguajes2023Hotel@outlook.com");
                
                this.generarEmail(reserva, paquete, cliente);
                string html = "A continuacion se detalla la reserva";
                
                email.IsBodyHtml = true;

                email.Priority = MailPriority.Normal;

                LinkedResource pdf = new LinkedResource("A:\\UCR\\Lenguajes para aplicaciones\\Proyectos\\DesarrolloWeb\\AppWebHotelBeach\\AppWebHotelBeach\\wwwroot\\PDF\\" + reserva.ID, MediaTypeNames.Application.Pdf);

                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);
                
                view.LinkedResources.Add(pdf);

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

        private void generarEmail(Reserva reserva, Paquete paquete, Cliente cliente)
        {
            try
            {
                PdfReserva pdfReserva = new PdfReserva();
                pdfReserva.GenerarPdfReserva(reserva, paquete, cliente);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }               
    }
}
