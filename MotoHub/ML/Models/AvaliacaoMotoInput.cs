using Microsoft.ML.Data;

namespace MotoHub.ML.Models
{
    public class AvaliacaoMotoInput
    {
        [LoadColumn(0)] public float Ano { get; set; }
        [LoadColumn(1)] public float Km { get; set; }
        [LoadColumn(2)] public float NumeroDeManutencoes { get; set; }
        [LoadColumn(3)] public float Pontuacao { get; set; }
    }
}
