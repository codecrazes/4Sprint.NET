using MotoHub.Dto.V2.Requests;

namespace MotoHub.Dto.V2.Requests
{
    public class ManutencaoRequestDto
    {
        public int MotoId { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public decimal Custo { get; set; }
    }
}
