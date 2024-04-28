using AppWebHotelBeach.Controllers;
using AppWebHotelBeach.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiHotelBeach.Models
{
    public class Reserva
    {               
        public Reserva()
        {

        }

        [Key]
        public int ID { get; set; }
        public int NumeroHabitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int CantidadPersonas { get; set; }
        public string MetodoPago { get; set; }
        public string? NumeroCheque { get; set; }
        public string? BancoCheque { get; set; }      
        public double Subtotal { get; set; }
        public double Descuento { get; set; }
        public double MontoTotalColones { get; set; }
        public double MontoTotalDolares { get; set; }
        public double Prima { get; set; }
        public double Cuota { get; set; }
        public int PaqueteID { get; set; }
        public int ClienteID { get; set; }
        


        public string EstadoPago
        {
            get
            {
                if((MetodoPago == "Cheque") || (MetodoPago == "Tarjeta"))
                {
                    return "Confirmado";
                }
                else
                {
                    return "Pendiente";
                }
            }
        }
            
        public int CantidadNoches
        {
            get
            {
                TimeSpan duracion = FechaSalida.Date - FechaEntrada.Date;
                return duracion.Days;
            }
        }
                               
        public void CalcularSubtotal(Paquete tempPaquete)
        {         
               Subtotal = (tempPaquete.Costo * CantidadNoches) * CantidadPersonas;
                       
        }

        public void CalcularDescuento()
        {
            
                if (MetodoPago == "Efectivo")
                {
                    if (CantidadNoches > 13)
                    {
                        Descuento = Subtotal * 0.25;
                    }
                    if ((CantidadNoches < 13) && (CantidadNoches > 9))
                    {
                        Descuento = Subtotal * 0.20;
                    }
                    if ((CantidadNoches < 10) && (CantidadNoches > 6))
                    {
                        Descuento = Subtotal * 0.15 ;
                    }
                    if ((CantidadNoches < 7) && (CantidadNoches > 2))
                    {
                        Descuento = Subtotal * 0.1;
                    }
                    else
                    {
                        Descuento = 0;
                    }
                }
                else
                {
                    Descuento = 0;
                }
            }

        public void CalcularMontoTotalDolares()
        {
            MontoTotalDolares = (Subtotal - Descuento);
            double IVA = MontoTotalDolares * 0.13;

            MontoTotalDolares = MontoTotalDolares + IVA;
            MontoTotalDolares = Math.Round(MontoTotalDolares, 2);
        }


        public void CalcularPrima(Paquete tempPaquete)
        {
            Prima = (MontoTotalDolares * tempPaquete.Prima) / 100;
            Prima = Math.Round(Prima, 2);
        }

        public void CalcularCuota(Paquete tempPaquete)
        {           
            Cuota = (MontoTotalDolares - Prima) / tempPaquete.CantidadCuotas;
            Cuota = Math.Round(Cuota, 2);
        }

        public void CalcularMontoTotalColones(double tipoCambio)
        {
            MontoTotalColones = tipoCambio * (MontoTotalDolares - Prima);
            MontoTotalColones = Math.Round(MontoTotalColones, 2);
        }

        private void AsignarID(Paquete tempPaquete, Cliente tempCliente)
        {
            PaqueteID = tempPaquete.ID;
            ClienteID = tempCliente.ID;
        }

        //Metodo para calcular toda la factura
        public void CalcularFactura(double tipoCambio, Paquete tempPaquete,Cliente tempCliente)
        {
            this.CalcularSubtotal(tempPaquete);
            this.CalcularDescuento();
            this.CalcularMontoTotalDolares();
            this.CalcularPrima(tempPaquete);
            this.CalcularCuota(tempPaquete);
            this.CalcularMontoTotalColones(tipoCambio);
            this.AsignarID(tempPaquete, tempCliente);
        }

        

        

         
        

        

        /*public bool ValidarDisponibilidad(DateTime fechaInicio, DateTime fechaFin)
          {
            bool fechasValidas = FechaEntrada <= fechaFin && FechaSalida >= fechaInicio;
            List<Reserva> reservasExistentes = ObtenerReservasHabitacion(NumeroHabitacion);
            bool disponibilidad = !reservasExistentes.Any(r => r.ID != ID && r.ValidarSuperposicionFechas(fechaInicio, fechaFin));
            return fechasValidas && disponibilidad;
          }

        private bool ValidarSuperposicionFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return FechaEntrada <= fechaFin && FechaSalida >= fechaInicio;
        }*/

       

    }
}
