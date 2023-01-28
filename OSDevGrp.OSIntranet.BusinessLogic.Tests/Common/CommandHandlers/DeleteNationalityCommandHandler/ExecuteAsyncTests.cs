using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.DeleteNationalityCommandHandler
{
	[TestFixture]
	public class ExecuteAsyncTests
	{
		#region Private variables

		private Mock<IValidator> _validatorMock;
		private Mock<ICommonRepository> _commonRepositoryMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_validatorMock = new Mock<IValidator>();
			_commonRepositoryMock = new Mock<ICommonRepository>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
		{
			ICommandHandler<IDeleteNationalityCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnDeleteNationalityCommand()
		{
			ICommandHandler<IDeleteNationalityCommand> sut = CreateSut();

			Mock<IDeleteNationalityCommand> deleteNationalityCommandMock = BuildDeleteNationalityCommandMock();
			await sut.ExecuteAsync(deleteNationalityCommandMock.Object);

			deleteNationalityCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<INationality>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertNumberWasCalledOnDeleteNationalityCommand()
		{
			ICommandHandler<IDeleteNationalityCommand> sut = CreateSut();

			Mock<IDeleteNationalityCommand> deleteNationalityCommandMock = BuildDeleteNationalityCommandMock();
			await sut.ExecuteAsync(deleteNationalityCommandMock.Object);

			deleteNationalityCommandMock.Verify(m => m.Number, Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDeleteNationalityAsyncOnCommonRepositoryWithNumberFromDeleteNationalityCommand()
		{
			ICommandHandler<IDeleteNationalityCommand> sut = CreateSut();

			int number = _fixture.Create<int>();
			await sut.ExecuteAsync(BuildUpdateNationalityCommand(number));

			_commonRepositoryMock.Verify(m => m.DeleteNationalityAsync(It.Is<int>(value => value == number)), Times.Once());
		}

		private ICommandHandler<IDeleteNationalityCommand> CreateSut()
		{
			_commonRepositoryMock.Setup(m => m.DeleteNationalityAsync(It.IsAny<int>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.Common.CommandHandlers.DeleteNationalityCommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
		}

		private IDeleteNationalityCommand BuildUpdateNationalityCommand(int? number = null)
		{
			return BuildDeleteNationalityCommandMock(number).Object;
		}

		private Mock<IDeleteNationalityCommand> BuildDeleteNationalityCommandMock(int? number = null)
		{
			Mock<IDeleteNationalityCommand> deleteNationalityCommandMock = new Mock<IDeleteNationalityCommand>();
			deleteNationalityCommandMock.Setup(m => m.Number)
				.Returns(number ?? _fixture.Create<int>());
			return deleteNationalityCommandMock;
		}
	}
}