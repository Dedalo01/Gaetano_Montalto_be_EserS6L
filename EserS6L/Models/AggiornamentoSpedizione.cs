using System;

namespace EserS6L.Models
{
    public class AggiornamentoSpedizione
    {
        public int Id { get; set; }
        public string NomeStato { get; set; }
        public string LuogoPacco { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataAggiornamento { get; set; }
        public int SpedizioneId { get; set; }
    }
}