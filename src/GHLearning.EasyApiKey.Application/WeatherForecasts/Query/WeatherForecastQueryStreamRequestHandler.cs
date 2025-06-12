using System.Runtime.CompilerServices;
using GHLearning.EasyApiKey.Core.WeatherForecasts;
using MediatR;

namespace GHLearning.EasyApiKey.Application.WeatherForecasts.Query;

internal class WeatherForecastQueryStreamRequestHandler(
	IWeatherForecastRepository weatherForecastRepository) : IStreamRequestHandler<WeatherForecastQueryStreamRequest, WeatherForecastQueryResponse>
{
	public async IAsyncEnumerable<WeatherForecastQueryResponse> Handle(WeatherForecastQueryStreamRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var weatherForecasts = await weatherForecastRepository.QueryAsync(request.Count, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);

		foreach (var weatherForecast in weatherForecasts)
		{
			yield return new WeatherForecastQueryResponse(
				weatherForecast.Date,
				weatherForecast.TemperatureC,
				weatherForecast.TemperatureF,
				weatherForecast.Summary);
		}
	}
}
