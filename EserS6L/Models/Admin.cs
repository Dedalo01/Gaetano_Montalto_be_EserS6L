using System.ComponentModel.DataAnnotations;

namespace EserS6L.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Massimo 50 caratteri.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(20, ErrorMessage = "Massimo 20 caratteri.")]
        public string Password { get; set; }
    }
}