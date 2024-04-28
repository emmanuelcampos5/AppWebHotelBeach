using System.ComponentModel.DataAnnotations;

namespace AppWebHotelBeach.Models
{
    public class SeguridadRestablecer
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite la contraseña enviada por email")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Digite la nueva contraseña")]
        [DataType(DataType.Password)]
        public string NuevoPassword { get; set; }


        [Required(ErrorMessage = "Confirme la contraseña")]
        [DataType(DataType.Password)]
        public string Confirmar { get; set; }
    }
}
