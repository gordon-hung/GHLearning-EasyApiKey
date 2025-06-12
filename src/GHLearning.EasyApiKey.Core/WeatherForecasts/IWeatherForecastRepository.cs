namespace GHLearning.EasyApiKey.Core.WeatherForecasts;

public interface IWeatherForecastRepository
{
	IAsyncEnumerable<WeatherForecastEntity> QueryAsync(int count, CancellationToken cancellationToken = default);
}
