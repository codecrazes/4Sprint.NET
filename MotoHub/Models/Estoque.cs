using MotoHub.Models;

namespace MotoHub.Models
{
    public class Estoque
    {
        public int Id { get; set; }
        public int MotoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }

        public Moto? Moto { get; set; }
    }
}
