﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableToken
{
	[TestFixture]
    public class ToByteArrayTests
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
		public void ToByteArray_WhenCalled_ReturnsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			byte[] result = sut.ToByteArray();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsNonEmptyByteArray()
		{
			IRefreshableToken sut = CreateSut();

			byte[] result = sut.ToByteArray();

			Assert.That(result, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereTokenTypeIsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereTokenTypeIsSerialized()
		{
			string tokenType = _fixture.Create<string>();
			IRefreshableToken sut = CreateSut(tokenType);

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereAccessTokenIsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereAccessTokenIsSerialized()
		{
			string accessToken = _fixture.Create<string>();
			IRefreshableToken sut = CreateSut(accessToken: accessToken);

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereRefreshTokenIsNotNull()
		{
			IRefreshableToken sut = CreateSut();

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereRefreshTokenIsSerialized()
		{
			string refreshToken = _fixture.Create<string>();
			IRefreshableToken sut = CreateSut(refreshToken: refreshToken);

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void ToByteArray_WhenCalled_ReturnsByteArrayWhereExpiresIsSerialized()
		{
			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IRefreshableToken sut = CreateSut(expires: expires);

			byte[] result = sut.ToByteArray();

			Assert.That(Deserialize(result).Expires, Is.EqualTo(expires));
		}

		private IRefreshableToken CreateSut(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			return new Domain.Security.RefreshableToken(tokenType ?? _fixture.Create<string>(), accessToken ?? _fixture.Create<string>(), refreshToken ?? _fixture.Create<string>(), expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
		}

		private static IRefreshableToken Deserialize(byte[] byteArray)
		{
			NullGuard.NotNull(byteArray, nameof(byteArray));

			return RefreshableTokenFactory.Create().FromByteArray(byteArray);
		}
	}
}