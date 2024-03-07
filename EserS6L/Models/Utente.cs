using System.ComponentModel.DataAnnotations;

namespace EserS6L.Models
{
    public class Utente
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Massimo 1000 caratteri.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Massimo 50 caratteri.")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Massimo 500 caratteri.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Inserire 16 caratteri.")]
        public string CodiceFiscale { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Inserire 11 caratteri.")]
        public string PartitaIva { get; set; }
        public string TipoCliente { get; set; }
        public int? AdminId { get; set; }
    }
}