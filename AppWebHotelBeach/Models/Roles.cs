using System.ComponentModel.DataAnnotations;

namespace AppWebHotelBeach.Models
{
    public class Roles
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string NombreRole { get; set; }
    }
}
