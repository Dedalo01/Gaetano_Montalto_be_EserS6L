using System;
using System.ComponentModel.DataAnnotations;

namespace EserS6L.Models
{
    public class Spedizione
    {
        [Key]
        public int Id { get; set; }


        public int ClienteId { get; set; }
        public DateTime DataSpedizione { get; set; }
        public double Peso { get; set; }

        [Required(ErrorMessage = "Campo Obbligatorio.")]
        [StringLength(50, ErrorMessage = "Massimo 1000 caratteri.")]
        public string CittaDestinataria { get; set; }
        public string Indirizzo { get; set; }
        public string NominativoDestinatario { get; set; }
        public double CostoSpedizione { get; set; }
        public DateTime DataConsegnaPrevista { get; set; }
    }
}