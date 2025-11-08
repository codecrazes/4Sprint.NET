using MotoHub.Dto.V2.Responses;

namespace MotoHub.Dto.V2.Responses
{
    public class AluguelResponseDto
    {
        public int Id { get; set; }
        public MotoResponseDto Moto { get; set; }
        public ClienteResponseDto Cliente { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
