using CorrelationId;
using CorrelationId.DependencyInjection;
using GHLearning.EasyApiKey.Core.ApiKeys;
using GHLearning.EasyApiKey.Core.WeatherForecasts;
using GHLearning.EasyApiKey.Infrastructure.ApiKeys;
using GHLearning.EasyApiKey.Infrastructure.Authorization;
using GHLearning.EasyApiKey.Infrastructure.Correlations;
using GHLearning.EasyApiKey.Infrastructure.WeatherForecasts;
using GHLearning.EasyApiKey.SharedKernel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.EasyApiKey.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
			Action<ApiKeyOptions, IServiceProvider> apiKeyOptions)
		=> services
		.AddOptions<ApiKeyOptions>()
		.Configure(apiKeyOptions)
		.Services
		.AddSingleton(TimeProvider.System)
		.AddAuthenticationInternal()
		.AddAuthorizationInternal()
		.AddCorrelation()
		.AddSingleton<ISequentialGuidGenerator, SequentialGuidGenerator>()
		.AddApiKeysInternal()
		.AddWeatherForecastsInternal();

	private static IServiceCollection AddApiKeysInternal(
		this IServiceCollection services)
		=> services.AddTransient<IApiKeyRepository, ApiKeyRepository>();

	private static IServiceCollection AddWeatherForecastsInternal(
		this IServiceCollection services)
		=> services.AddTransient<IWeatherForecastRepository, WeatherForecastRepository>();

	private static IServiceCollection AddAuthenticationInternal(
		this IServiceCollection services)
		=> services.AddAuthentication(options =>
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer()
		.Services
		.AddHttpContextAccessor();

	private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
		=> services
		.AddAuthorization(options => options.AddPolicy("ApiKeyPolicy", policy =>
		{
			policy.AddAuthenticationSchemes([JwtBearerDefaults.AuthenticationScheme]);
			policy.Requirements.Add(new ApiKeyRequirement());
		}))
		.AddTransient<IAuthorizationHandler, ApiKeyAuthorizationHandler>();

	private static IServiceCollection AddCorrelation(this IServiceCollection services)
		=> services.AddCorrelationId<CustomCorrelationIdProvider>(options =>
		{
			options.AddToLoggingScope = true;
			options.LoggingScopeKey = CorrelationIdOptions.DefaultHeader;
		})
		.Services;
}
