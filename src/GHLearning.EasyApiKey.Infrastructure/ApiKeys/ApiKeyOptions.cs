using GHLearning.EasyApiKey.Core.ApiKeys;

namespace GHLearning.EasyApiKey.Infrastructure.ApiKeys;
public record ApiKeyOptions
{
	public IReadOnlyCollection<ApiKeyEntity> ApiKeys { get; set; } = [];
}
