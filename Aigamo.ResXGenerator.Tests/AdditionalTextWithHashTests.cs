using Aigamo.ResXGenerator.Tools;
using FluentAssertions;
using Xunit;

namespace Aigamo.ResXGenerator.Tests;

public class AdditionalTextWithHashTests
{
	[Fact]
	public void SameFilePathAndContents_ShouldProduceSameHash()
	{
		var first = new AdditionalTextStub("Resources/Test.resx", "<root />");
		var second = new AdditionalTextStub("Resources/Test.resx", "<root />");

		var firstValue = new AdditionalTextWithHash(first);
		var secondValue = new AdditionalTextWithHash(second);

		firstValue.Hash.Should().Be(secondValue.Hash);
	}

	[Fact]
	public void DifferentContents_ShouldProduceDifferentHash()
	{
		var first = new AdditionalTextStub("Resources/Test.resx", "<root />");
		var second = new AdditionalTextStub("Resources/Test.resx", "<root><item /></root>");

		var firstValue = new AdditionalTextWithHash(first);
		var secondValue = new AdditionalTextWithHash(second);

		firstValue.Hash.Should().NotBe(secondValue.Hash);
	}
}
