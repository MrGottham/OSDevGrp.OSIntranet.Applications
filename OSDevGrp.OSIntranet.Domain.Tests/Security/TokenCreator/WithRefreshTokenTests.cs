using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
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
		public void WithRefreshToken_WhenRefreshTokenIsNull_ThrowsNotSupportedException()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(null));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsNull_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Refresh token is not supported for {typeof(IToken)}."));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsEmpty_ThrowsNotSupportedException()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(string.Empty));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsEmpty_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(string.Empty));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Refresh token is not supported for {typeof(IToken)}."));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsWhiteSpace_ThrowsNotSupportedException()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(" "));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenRefreshTokenIsWhiteSpace_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(" "));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Refresh token is not supported for {typeof(IToken)}."));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenCalled_ThrowsNotSupportedException()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(_fixture.Create<string>()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithRefreshToken_WhenCalled_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.WithRefreshToken(_fixture.Create<string>()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Refresh token is not supported for {typeof(IToken)}."));
			// ReSharper restore PossibleNullReferenceException
		}

		private static ITokenBuilder<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}
	}
}