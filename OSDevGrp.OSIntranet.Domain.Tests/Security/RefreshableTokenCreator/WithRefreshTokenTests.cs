using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
{
	[TestFixture]
	public class WithRefreshTokenTests
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
		public void WithRefreshToken_WhenRefreshTokenIsNull_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRefreshToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsEmpty_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRefreshToken(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsWhiteSpace_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithRefreshToken(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenCalled_ReturnsNotNull()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithRefreshToken(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenCalled_ReturnsSameTokenBuilder()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithRefreshToken(_fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private static ITokenBuilder<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}
	}
}