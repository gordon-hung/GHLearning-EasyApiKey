using GHLearning.EasyApiKey.Core.WeatherForecasts;

namespace GHLearning.EasyApiKey.Infrastructure.WeatherForecasts;

internal class WeatherForecastRepository : IWeatherForecastRepository
{
	public IAsyncEnumerable<WeatherForecastEntity> QueryAsync(int count, CancellationToken cancellationToken = default)
		=> Enumerable.Range(1, count).Select(index => new WeatherForecastEntity
		{
			Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			TemperatureC = Random.Shared.Next(-20, 55),
			Summary = WeatherForecastSummary.Summaries[Random.Shared.Next(WeatherForecastSummary.Summaries.Length)]
		}).ToAsyncEnumerable();
}
