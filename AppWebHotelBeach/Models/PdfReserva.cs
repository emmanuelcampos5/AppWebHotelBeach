
using iTextSharp.text;
using iTextSharp.text.pdf;
using ApiHotelBeach.Models;

namespace AppWebHotelBeach.Models
{
    public class PdfReserva
    {
        public void GenerarPdfReserva(Reserva reserva, Paquete paquete, Cliente cliente)
        {
            try
            {
                Document document = new Document();

                string ruta = "A:\\UCR\\Lenguajes para aplicaciones\\Proyectos\\DesarrolloWeb\\AppWebHotelBeach\\AppWebHotelBeach\\wwwroot\\PDF\\" + reserva.ID;
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(ruta, FileMode.Create));

                document.Open();

                document.NewPage();

                // Definir estilos de fuente
                Font fontTitle = FontFactory.GetFont(FontFactory.TIMES_BOLD, 16, Font.BOLD);
                Font fontSubtitle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14,Font.BOLDITALIC);
                Font fontBody = FontFactory.GetFont(FontFactory.TIMES, 12, Font.NORMAL);
                Font fontFooter = FontFactory.GetFont(FontFactory.TIMES_ITALIC, 12, Font.NORMAL);

                // Agregar contenido al documento
                document.Add(new Paragraph("Reserva Exitosa, A continuación se muestran los detalles de la Factura de su reserva",fontTitle));
                document.Add(new Paragraph("Factura Hotel Beach.SA", fontSubtitle));
                document.Add(new Paragraph("Fecha de emisión: " + DateTime.Now, fontBody));
                document.Add(new Paragraph("Nombre: " + cliente.NombreCliente,fontBody));
                document.Add(new Paragraph("Número de la Habitación: " + reserva.NumeroHabitacion.ToString() + "   " + "Cantidad de Personas: " + reserva.CantidadPersonas.ToString(), fontBody));
                document.Add(new Paragraph());
                document.Add(new Paragraph("Costo del día:" + paquete.Costo, fontBody));
                document.Add(new Paragraph("Fecha de Entrada_______________" + reserva.FechaEntrada,fontBody));
                document.Add(new Paragraph("Fecha de Salida________________" + reserva.FechaSalida,(fontBody)));
                document.Add(new Paragraph("Cantidad de Noches_____________" + reserva.CantidadNoches.ToString(), fontBody));
                document.Add(new Paragraph());
                document.Add(new Paragraph("Tipo de Paquete: " + paquete.TipoPaquete,fontBody));
                document.Add(new Paragraph());
                document.Add(new Paragraph("***************************************************************************************",fontFooter));
                document.Add(new Paragraph("Subtotal_______________________________$" + reserva.Subtotal, fontBody));
                document.Add(new Paragraph("Debe pagar cuotas de___________________$" + reserva.Cuota, fontBody));
                document.Add(new Paragraph("Prima__________________________________$" + reserva.Prima, fontBody));
                document.Add(new Paragraph("Costo con Descuento Aplicado___________$" + reserva.Descuento, fontBody));
                document.Add(new Paragraph("Monto Total en dólares_________________$" + reserva.MontoTotalDolares.ToString(),fontBody));
                document.Add(new Paragraph("Monto Total en Colones_________________₡" + reserva.MontoTotalColones.ToString(),fontBody));
                document.Add(new Paragraph());
                document.Add(new Paragraph("Método Pago:" + reserva.MetodoPago, fontBody));
                document.Add(new Paragraph("GRACIAS POR RESERVAR CON HOTELES BEACH.SA",fontFooter));


                //document.Add(new Paragraph("Reserva Exitosa, Aca tiene los datos de la reserva"));
                //document.Add(new Paragraph("Nombre de la persona que reservó: " + cliente.NombreCliente));
                //document.Add(new Paragraph("Numero de la Habitacion: " + reserva.NumeroHabitacion.ToString()));
                //document.Add(new Paragraph("Fecha de Entrada: " + reserva.FechaEntrada));
                //document.Add(new Paragraph("Fecha de Salida: " + reserva.FechaSalida));
                //document.Add(new Paragraph("Cantidad de Personas: " + reserva.CantidadPersonas.ToString()));
                //document.Add(new Paragraph("Cantidad de Noches: " + reserva.CantidadNoches.ToString()));
                //document.Add(new Paragraph("Tipo de Paquete: " + paquete.TipoPaquete));
                //document.Add(new Paragraph("Monto Total en dolares: $" + reserva.MontoTotalDolares.ToString()));
                //document.Add(new Paragraph("Monto Total en Colones: " + reserva.MontoTotalColones.ToString()));
                //document.Add(new Paragraph("GRACIAS POR RESERVAR CON HOTELES BEACH.SA"));

                document.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }           
}
