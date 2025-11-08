using System;

namespace MotoHub.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public int MotoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal Valor { get; set; }

        public Moto Moto { get; set; }
        public Cliente Cliente { get; set; }
    }
}
