using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class WithExpiresTests
	{
		#region Private variables

		private Fixture _fixture;
		private Random _random;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_fixture = new Fixture();
			_random = new Random(_fixture.Create<int>());
		}

		[Test]
		[Category("UnitTest")]
		public void WithExpires_WhenCalled_ReturnsNotNull()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			ITokenBuilder<IToken> result = sut.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithExpires_WhenCalled_ReturnsSameTokenBuilder()
		{
			ITokenBuilder<IToken> sut = CreateSut();

			ITokenBuilder<IToken> result = sut.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			Assert.That(result, Is.SameAs(sut));
		}

		private static ITokenBuilder<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}
	}
}