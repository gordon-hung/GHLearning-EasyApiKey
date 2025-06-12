using System.ComponentModel.DataAnnotations;

namespace GHLearning.EasyApiKey.WebApi.Controllers.WeatherForecasts.ViewModels;

public record WeatherForecastQueryViewModel
{
	[Required]
	[Range(minimum: 1, maximum: 30)]
	public int Count { get; init; }
}
