using MotoHub.Dto.V2.Requests;

namespace MotoHub.Dto.V2.Requests
{
    public class MotoRequestDto
    {
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public string Placa { get; set; }
        public decimal Preco { get; set; }
    }
}
