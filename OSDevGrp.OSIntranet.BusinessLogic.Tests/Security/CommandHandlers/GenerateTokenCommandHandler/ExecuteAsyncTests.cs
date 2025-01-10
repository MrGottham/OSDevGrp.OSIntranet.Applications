using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.CommandHandlers.GenerateTokenCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IClaimsIdentityResolver> _claimsIdentityResolverMock;
		private Mock<ITokenGenerator> _tokenGeneratorMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_claimsIdentityResolverMock = new Mock<IClaimsIdentityResolver>();
			_tokenGeneratorMock = new Mock<ITokenGenerator>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenGenerateTokenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IGenerateTokenCommand, IToken> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			Assert.That(result, Is.Not.Null);
			Assert.That(result.ParamName, Is.EqualTo("generateTokenCommand"));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertGetCurrentClaimsIdentityWasCalledOnClaimsIdentityResolver()
		{
			ICommandHandler<IGenerateTokenCommand, IToken> sut = CreateSut();

			await sut.ExecuteAsync(CreateGenerateTokenCommand());

			_claimsIdentityResolverMock.Verify(m => m.GetCurrentClaimsIdentity(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertGenerateWasCalledOnTokenGeneratorWithClaimsIdentityFromClaimsIdentityResolver()
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity();
			ICommandHandler<IGenerateTokenCommand, IToken> sut = CreateSut(claimsIdentity);

			await sut.ExecuteAsync(CreateGenerateTokenCommand());

            _tokenGeneratorMock.Verify(m => m.Generate(
                    It.Is<ClaimsIdentity>(value => value != null && value == claimsIdentity),
                    It.Is<TimeSpan>(value => (int) value.TotalSeconds == 3600),
					It.Is<string>(value => value == null)),
                Times.Once);
        }

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenTokenWasGenerated_ReturnsNotNull()
		{
			ICommandHandler<IGenerateTokenCommand, IToken> sut = CreateSut(generatesToken: true);

			IToken result = await sut.ExecuteAsync(CreateGenerateTokenCommand());

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenTokenWasGenerated_ReturnsGeneratedToken()
		{
			IToken token = _fixture.BuildTokenMock().Object;
			ICommandHandler<IGenerateTokenCommand, IToken> sut = CreateSut(generatesToken: true, token: token);

			IToken result = await sut.ExecuteAsync(CreateGenerateTokenCommand());

			Assert.That(result, Is.EqualTo(token));
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenNoTokenWasGenerated_ReturnsNull()
		{
			ICommandHandler<IGenerateTokenCommand, IToken> sut = CreateSut(generatesToken: false);

			IToken result = await sut.ExecuteAsync(CreateGenerateTokenCommand());

			Assert.That(result, Is.Null);
		}

		private ICommandHandler<IGenerateTokenCommand, IToken> CreateSut(ClaimsIdentity claimsIdentity = null, bool generatesToken = true, IToken token = null)
		{
			_claimsIdentityResolverMock.Setup(m => m.GetCurrentClaimsIdentity())
				.Returns(claimsIdentity ?? new ClaimsIdentity());

			_tokenGeneratorMock.Setup(m => m.Generate(It.IsAny<ClaimsIdentity>(), It.IsAny<TimeSpan>(), It.IsAny<string>()))
				.Returns(generatesToken ? token ?? _fixture.BuildTokenMock().Object : null);

			return new BusinessLogic.Security.CommandHandlers.GenerateTokenCommandHandler(_claimsIdentityResolverMock.Object, _tokenGeneratorMock.Object);
		}

		private IGenerateTokenCommand CreateGenerateTokenCommand()
		{
			return CreateGenerateTokenCommandMock().Object;
		}

		private Mock<IGenerateTokenCommand> CreateGenerateTokenCommandMock()
		{
			return new Mock<IGenerateTokenCommand>();
		}
	}
}