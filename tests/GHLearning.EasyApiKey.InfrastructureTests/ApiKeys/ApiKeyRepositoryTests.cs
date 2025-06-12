using GHLearning.EasyApiKey.Core.ApiKeys;
using GHLearning.EasyApiKey.Infrastructure.ApiKeys;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace GHLearning.EasyApiKey.InfrastructureTests.ApiKeys;

public class ApiKeyRepositoryTests
{
	[Fact]
	public async Task ValidationAsync_ReturnsTrue_WhenSecretExists()
	{
		// Arrange
		var options = Substitute.For<IOptions<ApiKeyOptions>>();
		options.Value.Returns(new ApiKeyOptions
		{
			ApiKeys = [new ApiKeyEntity { Secret = "test-secret" }]
		});

		var repository = new ApiKeyRepository(options);

		// Act
		var result = await repository.ValidationAsync("test-secret");

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task ValidationAsync_ReturnsFalse_WhenSecretDoesNotExist()
	{
		// Arrange
		var options = Substitute.For<IOptions<ApiKeyOptions>>();
		options.Value.Returns(new ApiKeyOptions
		{
			ApiKeys = [new ApiKeyEntity { Secret = "another-secret" }]
		});

		var repository = new ApiKeyRepository(options);

		// Act
		var result = await repository.ValidationAsync("not-exist");

		// Assert
		Assert.False(result);
	}
}
