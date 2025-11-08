using MotoHub.Dto.V2.Requests;

namespace MotoHub.Dto.V2.Requests
{
    public class VendaRequestDto
    {
        public int MotoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal Valor { get; set; }
    }
}
