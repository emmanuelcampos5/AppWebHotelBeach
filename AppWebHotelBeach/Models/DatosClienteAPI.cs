namespace AppWebHotelBeach.Models
{
        public class DatosClienteAPI
        {
            public string TipoIdentificacion { get; set; }
            public int ResultCount { get; set; }
            public string Nombre { get; set; }
            public string Database_Date { get; set; }
            public string License { get; set; }
            public string Cedula { get; set; }
            public string Query { get; set; }
            public List<Result> results { get; set; }  
        }

    
}
 
