namespace MotoHub.Models
{
    public class Manutencao
    {
        public int Id { get; set; }
        public int MotoId { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public decimal Custo { get; set; }

        public Moto Moto { get; set; }
    }
}
