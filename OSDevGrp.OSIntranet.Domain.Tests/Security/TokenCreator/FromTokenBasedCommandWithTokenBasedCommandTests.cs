using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class FromTokenBasedCommandWithTokenBasedCommandTests
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
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNull_ThrowsArgumentNullException()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromTokenBasedCommand((ITokenBasedCommand) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("tokenBasedCommand"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertTokenTypeWasCalledOnTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<ITokenBasedCommand> tokenBasedCommandMock = CreateTokenBasedCommandMock();
			sut.FromTokenBasedCommand(tokenBasedCommandMock.Object);

			tokenBasedCommandMock.Verify(m => m.TokenType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertAccessTokenWasCalledOnTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<ITokenBasedCommand> tokenBasedCommandMock = CreateTokenBasedCommandMock();
			sut.FromTokenBasedCommand(tokenBasedCommandMock.Object);

			tokenBasedCommandMock.Verify(m => m.AccessToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertExpiresWasCalledOnTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<ITokenBasedCommand> tokenBasedCommandMock = CreateTokenBasedCommandMock();
			sut.FromTokenBasedCommand(tokenBasedCommandMock.Object);

			tokenBasedCommandMock.Verify(m => m.Expires, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToTokenTypeOnTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand(tokenType);
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereAccessTokenIsEqualToAccessTokenOnTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand(accessToken: accessToken);
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereExpiresIsEqualToExpiresOnTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand(expires: expires);
			IToken result = sut.FromTokenBasedCommand(tokenBasedCommand);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}

		private ITokenBasedCommand CreateTokenBasedCommand(string tokenType = null, string accessToken = null, DateTime? expires = null)
		{
			return CreateTokenBasedCommandMock(tokenType, accessToken, expires).Object;
		}

		private Mock<ITokenBasedCommand> CreateTokenBasedCommandMock(string tokenType = null, string accessToken = null, DateTime? expires = null)
		{
			Mock<ITokenBasedCommand> tokenBasedCommandMock = new Mock<ITokenBasedCommand>();
			tokenBasedCommandMock.Setup(m => m.TokenType)
				.Returns(tokenType ?? _fixture.Create<string>());
			tokenBasedCommandMock.Setup(m => m.AccessToken)
				.Returns(accessToken ?? _fixture.Create<string>());
			tokenBasedCommandMock.Setup(m => m.Expires)
				.Returns(expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
			return tokenBasedCommandMock;
		}
	}
}