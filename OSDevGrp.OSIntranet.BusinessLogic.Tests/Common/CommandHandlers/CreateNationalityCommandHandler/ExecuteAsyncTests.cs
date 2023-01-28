using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.CommandHandlers.CreateNationalityCommandHandler
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
			ICommandHandler<ICreateNationalityCommand> sut = CreateSut();

			ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

			// ReSharper disable PossibleNullReferenceException
			Assert.That(result.ParamName, Is.EqualTo("command"));
			// ReSharper restore PossibleNullReferenceException
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnCreateNationalityCommand()
		{
			ICommandHandler<ICreateNationalityCommand> sut = CreateSut();

			Mock<ICreateNationalityCommand> createNationalityCommandMock = BuildCreateNationalityCommandMock();
			await sut.ExecuteAsync(createNationalityCommandMock.Object);

			createNationalityCommandMock.Verify(m => m.Validate(
					It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
					It.IsNotNull<Func<int, Task<INationality>>>()),
				Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnCreateNationalityCommand()
		{
			ICommandHandler<ICreateNationalityCommand> sut = CreateSut();

			Mock<ICreateNationalityCommand> createNationalityCommandMock = BuildCreateNationalityCommandMock();
			await sut.ExecuteAsync(createNationalityCommandMock.Object);

			createNationalityCommandMock.Verify(m => m.ToDomain(), Times.Once());
		}

		[Test]
		[Category("UnitTest")]
		public async Task ExecuteAsync_WhenCalled_AssertToCreateNationalityAsyncOnCommonRepositoryWithNationalityFromCreateNationalityCommand()
		{
			ICommandHandler<ICreateNationalityCommand> sut = CreateSut();

			INationality nationality = _fixture.BuildNationalityMock().Object;
			await sut.ExecuteAsync(BuildCreateNationalityCommand(nationality));

			_commonRepositoryMock.Verify(m => m.CreateNationalityAsync(It.Is<INationality>(value => value != null && value == nationality)), Times.Once());
		}

		private ICommandHandler<ICreateNationalityCommand> CreateSut()
		{
			_commonRepositoryMock.Setup(m => m.CreateNationalityAsync(It.IsAny<INationality>()))
				.Returns(Task.CompletedTask);

			return new BusinessLogic.Common.CommandHandlers.CreateNationalityCommandHandler(_validatorMock.Object, _commonRepositoryMock.Object);
		}

		private ICreateNationalityCommand BuildCreateNationalityCommand(INationality nationality = null)
		{
			return BuildCreateNationalityCommandMock(nationality).Object;
		}

		private Mock<ICreateNationalityCommand> BuildCreateNationalityCommandMock(INationality nationality = null)
		{
			Mock<ICreateNationalityCommand> createNationalityCommandMock = new Mock<ICreateNationalityCommand>();
			createNationalityCommandMock.Setup(m => m.ToDomain())
				.Returns(nationality ?? _fixture.BuildNationalityMock().Object);
			return createNationalityCommandMock;
		}
	}
}