using GHLearning.EasyApiKey.Application.WeatherForecasts.Query;
using GHLearning.EasyApiKey.Infrastructure.Authorization;
using GHLearning.EasyApiKey.WebApi.Controllers.WeatherForecasts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyApiKey.WebApi.Controllers.WeatherForecasts;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuthorize("ApiKeyPolicy")]
public class WeatherForecastController() : ControllerBase
{
	[HttpGet(Name = "GetWeatherForecast")]
	public IAsyncEnumerable<WeatherForecastQueryResponse> QueryAsync(
		[FromServices] ISender sender,
		[FromQuery] WeatherForecastQueryViewModel source)
		=> sender.CreateStream(
			new WeatherForecastQueryStreamRequest(source.Count),
			cancellationToken: HttpContext.RequestAborted);
}
