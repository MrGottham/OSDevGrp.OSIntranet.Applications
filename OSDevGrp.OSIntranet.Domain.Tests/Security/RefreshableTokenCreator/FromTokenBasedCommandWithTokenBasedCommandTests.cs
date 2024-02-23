using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.RefreshableTokenCreator
{
	[TestFixture]
	public class FromTokenBasedCommandWithTokenBasedCommandTests
	{
		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNull_ThrowsNotSupportedException()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedCommand((ITokenBasedCommand) null));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNull_ThrowsNotSupportedExceptionWhereMessageIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedCommand((ITokenBasedCommand) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNull_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedCommand((ITokenBasedCommand) null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Cannot build an {nameof(IRefreshableToken)} from {nameof(ITokenBasedCommand)}."));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNotNull_ReturnsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedCommand(CreateTokenBasedCommand()));

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNotNull_ThrowsNotSupportedExceptionWhereMessageIsNotNull()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedCommand(CreateTokenBasedCommand()));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.Not.Null);
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public void FromTokenBasedCommand_WhenTokenBasedCommandIsNotNull_ThrowsNotSupportedExceptionWithInformativeMessage()
		{
			ITokenCreator<IRefreshableToken> sut = CreateSut();

			ITokenBasedCommand tokenBasedCommand = CreateTokenBasedCommand();
			NotSupportedException result = Assert.Throws<NotSupportedException>(() => sut.FromTokenBasedCommand(tokenBasedCommand));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.Message, Is.EqualTo($"Cannot build an {nameof(IRefreshableToken)} from {tokenBasedCommand.GetType().Name}."));
			// ReSharper restore PossibleNullReferenceException
		}

		private static ITokenCreator<IRefreshableToken> CreateSut()
		{
			return new Domain.Security.RefreshableTokenCreator();
		}

		private ITokenBasedCommand CreateTokenBasedCommand()
		{
			return CreateTokenBasedCommandMock().Object;
		}

		private Mock<ITokenBasedCommand> CreateTokenBasedCommandMock()
		{
			return new Mock<ITokenBasedCommand>();
		}
	}
}