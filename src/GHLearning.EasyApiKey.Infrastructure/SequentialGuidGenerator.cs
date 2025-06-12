using GHLearning.EasyApiKey.SharedKernel;

namespace GHLearning.EasyApiKey.Infrastructure;

internal class SequentialGuidGenerator : ISequentialGuidGenerator
{
	public Guid NewId() => SequentialGuid.SequentialGuidGenerator.Instance.NewGuid();
}
