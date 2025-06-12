using GHLearning.EasyApiKey.Core.ApiKeys;
using GHLearning.EasyApiKey.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GHLearning.EasyApiKey.Infrastructure.Authorization;

public class ApiKeyAuthorizationHandler(
	IHttpContextAccessor httpContextAccessor,
	IApiKeyRepository apiKeyRepository) : AuthorizationHandler<ApiKeyRequirement>
{
	protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
	{
		var httpContext = httpContextAccessor.HttpContext;

		if (httpContext != null)
		{
			if (httpContext.Request.Headers.TryGetValue(HttpHeaderConsts.ApiKeyHeaderName, out var extractedApiKey))
			{
				if (await apiKeyRepository.ValidationAsync(extractedApiKey!).ConfigureAwait(false))
				{
					context.Succeed(requirement);
				}
			}
		}
	}
}
