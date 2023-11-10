using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.TokenCreator
{
	[TestFixture]
	public class FromTokenBasedCommandWithRefreshableTokenBasedCommandTests
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
		public void FromTokenBasedCommand_WhenRefreshableTokenBasedCommandIsNull_ThrowsArgumentNullException()
		{
			ITokenCreator<IToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromTokenBasedCommand(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshableTokenBasedCommand"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertTokenTypeWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.TokenType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertAccessTokenWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.AccessToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertRefreshTokenWasNotCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.RefreshToken, Times.Never);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertExpiresWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.Expires, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereTokenTypeIsEqualToTokenTypeOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(tokenType);
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.AccessToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenCreator<IToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereAccessTokenIsEqualToAccessTokenOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(accessToken: accessToken);
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsTokenWhereExpiresIsEqualToExpiresOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(expires: expires);
			IToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IToken> CreateSut()
		{
			return new Domain.Security.TokenCreator();
		}

		private IRefreshableTokenBasedCommand CreateRefreshableTokenBasedCommand(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			return CreateRefreshableTokenBasedCommandMock(tokenType, accessToken, refreshToken, expires).Object;
		}

		private Mock<IRefreshableTokenBasedCommand> CreateRefreshableTokenBasedCommandMock(string tokenType = null, string accessToken = null, string refreshToken = null, DateTime? expires = null)
		{
			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = new Mock<IRefreshableTokenBasedCommand>();
			refreshableTokenBasedCommandMock.Setup(m => m.TokenType)
				.Returns(tokenType ?? _fixture.Create<string>());
			refreshableTokenBasedCommandMock.Setup(m => m.AccessToken)
				.Returns(accessToken ?? _fixture.Create<string>());
			refreshableTokenBasedCommandMock.Setup(m => m.RefreshToken)
				.Returns(refreshToken ?? _fixture.Create<string>());
			refreshableTokenBasedCommandMock.Setup(m => m.Expires)
				.Returns(expires ?? DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime());
			return refreshableTokenBasedCommandMock;
		}
	}
}