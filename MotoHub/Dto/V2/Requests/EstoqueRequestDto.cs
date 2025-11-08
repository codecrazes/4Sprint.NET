using MotoHub.Dto.V2.Requests;

namespace MotoHub.Dto.V2.Requests
{
    public class EstoqueRequestDto
    {
        public int MotoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
