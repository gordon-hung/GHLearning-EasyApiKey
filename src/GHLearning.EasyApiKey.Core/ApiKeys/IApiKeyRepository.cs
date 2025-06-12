namespace GHLearning.EasyApiKey.Core.ApiKeys;

public interface IApiKeyRepository
{
	Task<bool> ValidationAsync(string secret, CancellationToken cancellationToken = default);
}
