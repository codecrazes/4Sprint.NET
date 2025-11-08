using System;

namespace MotoHub.Models
{
    public class Aluguel
    {
        public int Id { get; set; }
        public int MotoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public Moto Moto { get; set; }
        public Cliente Cliente { get; set; }
    }
}
