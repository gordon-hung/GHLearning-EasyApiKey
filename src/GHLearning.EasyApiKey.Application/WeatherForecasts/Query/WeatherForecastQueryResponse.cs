namespace GHLearning.EasyApiKey.Application.WeatherForecasts.Query;
public record WeatherForecastQueryResponse(
	DateOnly Date,
	int TemperatureC,
	int TemperatureF,
	string? Summary);
