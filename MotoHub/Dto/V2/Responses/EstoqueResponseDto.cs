using MotoHub.Dto.V2.Responses;

namespace MotoHub.Dto.V2.Responses
{
    public class EstoqueResponseDto
    {
        public int Id { get; set; }
        public MotoResponseDto Moto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
