using MotoHub.Dto.V2.Responses;

namespace MotoHub.Dto.V2.Responses
{
    public class MotoResponseDto
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public string Placa { get; set; }
        public decimal Preco { get; set; }
    }
}
