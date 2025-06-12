using GHLearning.EasyApiKey.Application.WeatherForecasts.Query;
using GHLearning.EasyApiKey.Core.WeatherForecasts;
using NSubstitute;

namespace GHLearning.EasyApiKey.ApplicationTests.WeatherForecasts.Query;

public class WeatherForecastQueryStreamRequestHandlerTests
{
	[Fact]
	public async Task Handle_ValidRequest_ReturnsWeatherForecasts()
	{
		// Arrange
		var fakeWeatherForecastRepository = Substitute.For<IWeatherForecastRepository>();
		var handler = new WeatherForecastQueryStreamRequestHandler(
			fakeWeatherForecastRepository);
		var request = new WeatherForecastQueryStreamRequest(Count: 5);
		_ = fakeWeatherForecastRepository
			.QueryAsync(Arg.Is(request.Count), Arg.Any<CancellationToken>())
			.Returns(Enumerable.Range(1, request.Count).Select(index => new WeatherForecastEntity
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = WeatherForecastSummary.Summaries[Random.Shared.Next(WeatherForecastSummary.Summaries.Length)]
			}).ToAsyncEnumerable());
		// Act
		var result = await handler.Handle(request, CancellationToken.None).ToArrayAsync();
		// Assert
		Assert.NotNull(result);
		Assert.Equal(request.Count, result.Length);
		foreach (var forecast in result)
		{
			Assert.NotEqual(default, forecast.Date);
			Assert.InRange(forecast.TemperatureC, -20, 55);
			Assert.InRange(forecast.TemperatureF, -4, 131);
			Assert.Contains(forecast.Summary, WeatherForecastSummary.Summaries);
		}
	}

	[Fact]
	public async Task Handle_ZeroCountRequest_ReturnsEmpty()
	{
		// Arrange
		var fakeWeatherForecastRepository = Substitute.For<IWeatherForecastRepository>();
		var handler = new WeatherForecastQueryStreamRequestHandler(
			fakeWeatherForecastRepository);
		var request = new WeatherForecastQueryStreamRequest(Count: 0);
		_ = fakeWeatherForecastRepository
			.QueryAsync(Arg.Is(request.Count), Arg.Any<CancellationToken>())
			.Returns(AsyncEnumerable.Empty<WeatherForecastEntity>());
		// Act
		var result = await handler.Handle(request, CancellationToken.None).ToArrayAsync();
		// Assert
		Assert.NotNull(result);
		Assert.Empty(result);
	}

	[Fact]
	public async Task Handle_MinTemperature_ReturnsCorrectTemperatureF()
	{
		// Arrange
		var fakeWeatherForecastRepository = Substitute.For<IWeatherForecastRepository>();
		var handler = new WeatherForecastQueryStreamRequestHandler(fakeWeatherForecastRepository);
		var minTempC = -20;
		var expectedF = -3; // 攝氏 -20 度對應華氏 -3 度
		var request = new WeatherForecastQueryStreamRequest(Count: 1);
		_ = fakeWeatherForecastRepository
			.QueryAsync(Arg.Is(request.Count), Arg.Any<CancellationToken>())
			.Returns(new[]
			{
				new WeatherForecastEntity
				{
					Date = DateOnly.FromDateTime(DateTime.Now),
					TemperatureC = minTempC,
					Summary = WeatherForecastSummary.Summaries[0]
				}
			}.ToAsyncEnumerable());
		// Act
		var result = await handler.Handle(request, CancellationToken.None).ToArrayAsync();
		// Assert
		Assert.Single(result);
		Assert.Equal(minTempC, result[0].TemperatureC);
		Assert.Equal(expectedF, result[0].TemperatureF);
	}

	[Fact]
	public async Task Handle_MaxTemperature_ReturnsCorrectTemperatureF()
	{
		// Arrange
		var fakeWeatherForecastRepository = Substitute.For<IWeatherForecastRepository>();
		var handler = new WeatherForecastQueryStreamRequestHandler(fakeWeatherForecastRepository);
		var maxTempC = 55;
		var expectedF = 130; // 攝氏 55 度對應華氏 130 度
		var request = new WeatherForecastQueryStreamRequest(Count: 1);
		_ = fakeWeatherForecastRepository
			.QueryAsync(Arg.Is(request.Count), Arg.Any<CancellationToken>())
			.Returns(new[]
			{
				new WeatherForecastEntity
				{
					Date = DateOnly.FromDateTime(DateTime.Now),
					TemperatureC = maxTempC,
					Summary = WeatherForecastSummary.Summaries[0]
				}
			}.ToAsyncEnumerable());
		// Act
		var result = await handler.Handle(request, CancellationToken.None).ToArrayAsync();
		// Assert
		Assert.Single(result);
		Assert.Equal(maxTempC, result[0].TemperatureC);
		Assert.Equal(expectedF, result[0].TemperatureF);
	}
}
