using MotoHub.Dto.V2.Responses;

namespace MotoHub.Dto.V2.Responses
{
    public class VendaResponseDto
    {
        public int Id { get; set; }
        public MotoResponseDto Moto { get; set; }
        public ClienteResponseDto Cliente { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal Valor { get; set; }
    }
}
