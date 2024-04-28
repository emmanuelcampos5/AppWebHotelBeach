using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppWebHotelBeach.Models
{
    public class Cliente
    {

        [Key]
        public int ID { get; set; }


        [Required]
        public string Cedula { get; set; }


        [Required]
        [DisplayName("Tipo de cedula")]
        public string TipoCedula { get; set; }


        [Required]
        [DisplayName("Nombre Completo")]
        public string NombreCliente { get; set; }


        [Required]
        public string Telefono { get; set; }


        [Required]
        [DisplayName("Direccion")]
        public string DireccionCliente { get; set; }


        [Required]
        [DisplayName("Correo Electronico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required]
        [DisplayName("Fecha de registro")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        [Required]
        public int Restablecer { get; set; }

        [Required]
        public int RoleID { get; set; }
    }
}
