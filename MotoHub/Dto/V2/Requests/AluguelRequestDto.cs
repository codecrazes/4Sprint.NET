using MotoHub.Dto.V2.Requests;

namespace MotoHub.Dto.V2.Requests
{
    public class AluguelRequestDto
    {
        public int MotoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
