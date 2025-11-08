using Microsoft.ML;
using MotoHub.ML.Models;

namespace MotoHub.ML.Training
{
    public static class AvaliacaoMotoModelBuilder
    {
        public static void TreinarModelo()
        {
            var mlContext = new MLContext();

            var dados = new List<AvaliacaoMotoInput>
            {
                new() { Ano = 2024, Km = 5000, NumeroDeManutencoes = 0, Pontuacao = 9.8f },
                new() { Ano = 2022, Km = 20000, NumeroDeManutencoes = 1, Pontuacao = 8.2f },
                new() { Ano = 2020, Km = 40000, NumeroDeManutencoes = 2, Pontuacao = 7.0f },
                new() { Ano = 2018, Km = 60000, NumeroDeManutencoes = 3, Pontuacao = 5.5f },
                new() { Ano = 2015, Km = 90000, NumeroDeManutencoes = 4, Pontuacao = 4.0f }
            };

            var dadosTreino = mlContext.Data.LoadFromEnumerable(dados);

            var pipeline = mlContext.Transforms.Concatenate("Features", nameof(AvaliacaoMotoInput.Ano), nameof(AvaliacaoMotoInput.Km), nameof(AvaliacaoMotoInput.NumeroDeManutencoes))
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Pontuacao", maximumNumberOfIterations: 100));

            var modelo = pipeline.Fit(dadosTreino);

            mlContext.Model.Save(modelo, dadosTreino.Schema, "ML/Models/condicao_moto_model.zip");
        }
    }
}
