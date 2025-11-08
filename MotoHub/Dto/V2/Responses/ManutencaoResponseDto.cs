using MotoHub.Dto.V2.Responses;

namespace MotoHub.Dto.V2.Responses
{
    public class ManutencaoResponseDto
    {
        public int Id { get; set; }
        public MotoResponseDto Moto { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public decimal Custo { get; set; }
    }
}
