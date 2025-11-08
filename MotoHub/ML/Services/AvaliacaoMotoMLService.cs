using Microsoft.ML;
using MotoHub.Dto.V2.Requests;
using MotoHub.Dto.V2.Responses;
using MotoHub.ML.Models;

namespace MotoHub.ML.Services
{
	public class AvaliacaoMotoMLService
	{
		private readonly MLContext _mlContext;
		private readonly ITransformer _modelo;

		public AvaliacaoMotoMLService()
		{
			_mlContext = new MLContext();
			_modelo = _mlContext.Model.Load("ML/Models/condicao_moto_model.zip", out _);
		}

		public AvaliacaoMotoResponseDto AvaliarMoto(AvaliacaoMotoRequestDto dto)
		{
			var input = new AvaliacaoMotoInput
			{
				Ano = dto.Ano,
				Km = dto.Km,
				NumeroDeManutencoes = dto.NumeroDeManutencoes
			};

			var engine = _mlContext.Model.CreatePredictionEngine<AvaliacaoMotoInput, AvaliacaoMotoOutput>(_modelo);
			var resultado = engine.Predict(input);

			float score = MathF.Round(resultado.Score, 1);

			float idade = DateTime.Now.Year - dto.Ano;

			if (score < 6 && dto.Km < 10000 && idade <= 1)
			{
				score = 10f;
			}
			else if (score < 4 && idade <= 3 && dto.Km < 20000)
			{
				score = 8f; 
			}
			else if (score <= 0 || float.IsNaN(score))
			{
				score = 10f - (idade * 0.25f + dto.Km / 30000f + dto.NumeroDeManutencoes * 0.5f);
				score = MathF.Round(score, 1);
				score = score < 0 ? 0 : (score > 10 ? 10 : score);
			}

			string condicao = score switch
			{
				>= 8.0f => "Excelente",
				>= 6.0f => "Boa",
				>= 4.0f => "Regular",
				_ => "Ruim"
			};

			return new AvaliacaoMotoResponseDto
			{
				Condicao = condicao,
				Pontuacao = score
			};
		}
	}
}
