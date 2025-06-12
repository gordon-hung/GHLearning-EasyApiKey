using GHLearning.EasyApiKey.Core.ApiKeys;
using Microsoft.Extensions.Options;

namespace GHLearning.EasyApiKey.Infrastructure.ApiKeys;

internal class ApiKeyRepository(
	IOptions<ApiKeyOptions> apiKeyOptions) : IApiKeyRepository
{
	public Task<bool> ValidationAsync(string secret, CancellationToken cancellationToken = default)
		=> Task.FromResult(apiKeyOptions.Value.ApiKeys.Any(a => a.Secret == secret));
}
