using CorrelationId;
using GHLearning.EasyApiKey.Infrastructure.Correlations;
using GHLearning.EasyApiKey.SharedKernel;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace GHLearning.EasyApiKey.InfrastructureTests.Correlations;

public class CustomCorrelationIdProviderTests
{
	[Fact]
	public void GenerateCorrelationId_ShouldReturnFromHeader_IfHeaderExists()
	{
		// Arrange
		var fakeGuidGen = Substitute.For<ISequentialGuidGenerator>();
		var provider = new CustomCorrelationIdProvider(fakeGuidGen);
		var context = new DefaultHttpContext();
		var expectedId = "header-correlation-id";
		context.Request.Headers[CorrelationIdOptions.DefaultHeader] = expectedId;

		// Act
		var result = provider.GenerateCorrelationId(context);

		// Assert
		Assert.Equal(expectedId, result);
	}

	[Fact]
	public void GenerateCorrelationId_ShouldReturnFromItems_IfHeaderMissingButItemsExists()
	{
		// Arrange
		var fakeGuidGen = Substitute.For<ISequentialGuidGenerator>();
		var provider = new CustomCorrelationIdProvider(fakeGuidGen);
		var context = new DefaultHttpContext();
		var expectedId = "item-correlation-id";
		context.Items[CorrelationIdOptions.DefaultHeader] = expectedId;

		// Act
		var result = provider.GenerateCorrelationId(context);

		// Assert
		Assert.Equal(expectedId, result);
	}

	[Fact]
	public void GenerateCorrelationId_ShouldReturnNewGuid_IfHeaderAndItemsMissing()
	{
		// Arrange
		var fakeGuidGen = Substitute.For<ISequentialGuidGenerator>();
		var provider = new CustomCorrelationIdProvider(fakeGuidGen);
		var context = new DefaultHttpContext();
		var newGuid = Guid.NewGuid();
		fakeGuidGen.NewId().Returns(newGuid);

		// Act
		var result = provider.GenerateCorrelationId(context);

		// Assert
		Assert.Equal(newGuid.ToString(), result);
	}
}
