using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
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
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.FromTokenBasedCommand(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("refreshableTokenBasedCommand"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertTokenTypeWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.TokenType, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertAccessTokenWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.AccessToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertRefreshTokenWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.RefreshToken, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_AssertExpiresWasCalledOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			Mock<IRefreshableTokenBasedCommand> refreshableTokenBasedCommandMock = CreateRefreshableTokenBasedCommandMock();
			sut.FromTokenBasedCommand(refreshableTokenBasedCommandMock.Object);

			refreshableTokenBasedCommandMock.Verify(m => m.Expires, Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.TokenType, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsNotEmpty()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.TokenType, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereTokenTypeIsEqualToTokenTypeOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string tokenType = _fixture.Create<string>();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(tokenType);
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.TokenType, Is.EqualTo(tokenType));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsNotEmpty()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.AccessToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereAccessTokenIsEqualToAccessTokenOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string accessToken = _fixture.Create<string>();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(accessToken: accessToken);
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.AccessToken, Is.EqualTo(accessToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.RefreshToken, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsNotEmpty()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand();
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.RefreshToken, Is.Not.Empty);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereRefreshTokenIsEqualToRefreshTokenOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			string refreshToken = _fixture.Create<string>();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(refreshToken: refreshToken);
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.RefreshToken, Is.EqualTo(refreshToken));
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenCalled_ReturnsRefreshableTokenWhereExpiresIsEqualToExpiresOnRefreshableTokenBasedCommand()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 3600)).ToUniversalTime();
			IRefreshableTokenBasedCommand refreshableTokenBasedCommand = CreateRefreshableTokenBasedCommand(expires: expires);
			IRefreshableToken result = sut.FromTokenBasedCommand(refreshableTokenBasedCommand);

			Assert.That(result.Expires, Is.EqualTo(expires));
		}

		private static ITokenCreator<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
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