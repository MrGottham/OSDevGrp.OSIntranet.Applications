using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
{
	[TestFixture]
	public class WithTokenTypeTests
	{
		#region Private variables

		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void WithTokenType_WhenTokenTypeIsNull_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithTokenType(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithTokenType_WhenTokenTypeIsEmpty_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithTokenType(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithTokenType_WhenTokenTypeIsWhiteSpace_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithTokenType(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenType"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithTokenType_WhenCalled_ReturnsNotNull()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithTokenType(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithTokenType_WhenCalled_ReturnsSameTokenBuilder()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithTokenType(_fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private static ITokenBuilder<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}
	}
}