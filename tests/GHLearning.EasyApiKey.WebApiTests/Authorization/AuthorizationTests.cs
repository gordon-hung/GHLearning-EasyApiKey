using GHLearning.EasyApiKey.Core.ApiKeys;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace GHLearning.EasyApiKey.WebApiTests.Authorization;

public class AuthorizationTests
{
	[Fact]
	public async Task Authorization_ShouldReturnOk_WhenApiKeyHeaderIsValid()
	{
		var count = 1;
		var apiKey = "valid";
		var factory = new CustomWebApplicationFactory(builder =>
		{
			var fakeSender = Substitute.For<ISender>();
			var fakeApiKeyRepository = Substitute.For<IApiKeyRepository>();
			_ = fakeApiKeyRepository.ValidationAsync(
				Arg.Is(apiKey),
				Arg.Any<CancellationToken>())
			.Returns(true);
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender).AddTransient(_ => fakeApiKeyRepository));
		});
		var client = factory.CreateClient();
		client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
		var response = await client.GetAsync($"/api/WeatherForecast?count={count}");
		Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
	}

	[Fact]
	public async Task Authorization_ShouldReturnUnauthorized_WhenApiKeyHeaderIsMissing()
	{
		var count = 1;
		var factory = new CustomWebApplicationFactory(builder =>
		{
			var fakeSender = Substitute.For<ISender>();
			var fakeApiKeyRepository = Substitute.For<IApiKeyRepository>();
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender).AddTransient(_ => fakeApiKeyRepository));
		});
		var client = factory.CreateClient();
		var response = await client.GetAsync($"/api/WeatherForecast?count={count}");
		Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
	}

	[Fact]
	public async Task Authorization_ShouldReturnUnauthorized_WhenCredentialIsInvalid()
	{
		var count = 1;
		var apiKey = "invalid";
		var factory = new CustomWebApplicationFactory(builder =>
		{
			var fakeSender = Substitute.For<ISender>();
			var fakeApiKeyRepository = Substitute.For<IApiKeyRepository>();
			_ = fakeApiKeyRepository.ValidationAsync(
				Arg.Is(apiKey),
				Arg.Any<CancellationToken>())
			.Returns(false);
			_ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeSender).AddTransient(_ => fakeApiKeyRepository));
		});

		var client = factory.CreateClient();
		client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
		var response = await client.GetAsync($"/api/WeatherForecast?count={count}");

		Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
	}
}
