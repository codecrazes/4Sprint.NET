using MotoHub.Dto.V2.Responses;

namespace MotoHub.Dto.V2.Responses
{
    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}
