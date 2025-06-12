namespace GHLearning.EasyApiKey.Core.ApiKeys;
public record ApiKeyEntity
{
	public string Code { get; set; } = default!;
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string Secret { get; set; } = default!;
}
