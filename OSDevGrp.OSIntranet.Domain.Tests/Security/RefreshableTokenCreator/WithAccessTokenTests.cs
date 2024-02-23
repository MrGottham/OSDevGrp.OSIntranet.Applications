using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
{
	[TestFixture]
	public class WithAccessTokenTests
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
		public void WithAccessToken_WhenAccessTokenIsNull_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAccessToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("accessToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithAccessToken_WhenAccessTokenIsEmpty_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAccessToken(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("accessToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithAccessToken_WhenAccessTokenIsWhiteSpace_ThrowsArgumentNullException()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAccessToken(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("accessToken"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithAccessToken_WhenCalled_ReturnsNotNull()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithAccessToken(_fixture.Create<string>());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithAccessToken_WhenCalled_ReturnsSameTokenBuilder()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithAccessToken(_fixture.Create<string>());

			Assert.That(result, Is.SameAs(sut));
		}

		private static ITokenBuilder<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}
	}
}