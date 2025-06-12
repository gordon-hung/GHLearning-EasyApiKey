namespace GHLearning.EasyApiKey.Core.ApiKeys;
public record ApiKeyEntity
{
	public string Secret { get; set; } = default!;
}
