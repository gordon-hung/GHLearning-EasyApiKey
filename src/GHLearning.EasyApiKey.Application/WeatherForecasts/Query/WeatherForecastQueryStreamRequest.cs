using MediatR;

namespace GHLearning.EasyApiKey.Application.WeatherForecasts.Query;
public record WeatherForecastQueryStreamRequest(
	int Count) : IStreamRequest<WeatherForecastQueryResponse>;
