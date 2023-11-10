using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
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
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void WithExpires_WhenCalled_ReturnsSameTokenBuilder()
		{
			ITokenBuilder<IRefreshableToken> sut = CreateSut();

			ITokenBuilder<IRefreshableToken> result = sut.WithExpires(DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());

			Assert.That(result, Is.SameAs(sut));
		}

		private static ITokenBuilder<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}
	}
}