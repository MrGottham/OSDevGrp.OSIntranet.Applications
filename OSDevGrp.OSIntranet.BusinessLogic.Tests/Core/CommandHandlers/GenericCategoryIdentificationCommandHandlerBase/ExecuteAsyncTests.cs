using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers.GenericCategoryIdentificationCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands.IGenericCategoryIdentificationCommand<OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>, OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.CommandHandlers.GenericCategoryIdentificationCommandHandlerBase
{
    [TestFixture]
    public class ExecuteAsyncTests
    {
        #region Private variables

        private Mock<IValidator> _validatorMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator>();
        }

        [Test]
        [Category("UnitTest")]
        public void ExecuteAsync_WhenCommandIsNull_ThrowsArgumentNullException()
        {
            CommandHandler sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ExecuteAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("command"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnGenericCategoryIdentificationCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IGenericCategoryIdentificationCommand<IGenericCategory>> genericCategoryIdentificationCommandMock = CreateGenericCategoryIdentificationCommandMock();
            await sut.ExecuteAsync(genericCategoryIdentificationCommandMock.Object);

            genericCategoryIdentificationCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.IsNotNull<Func<int, Task<IGenericCategory>>>()),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnGenericCategoryIdentificationCommandHandlerBase()
        {
            CommandHandler sut = CreateSut();

            await sut.ExecuteAsync(CreateGenericCategoryIdentificationCommand());

            Assert.That(((MyGenericCategoryIdentificationCommandHandler)sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnGenericCategoryIdentificationCommandHandlerBaseWithSameGenericCategoryIdentificationCommand()
        {
            CommandHandler sut = CreateSut();

            IGenericCategoryIdentificationCommand<IGenericCategory> genericCategoryIdentificationCommand = CreateGenericCategoryIdentificationCommand();
            await sut.ExecuteAsync(genericCategoryIdentificationCommand);

            Assert.That(((MyGenericCategoryIdentificationCommandHandler)sut).ManageRepositoryAsyncWasCalledWithCommand, Is.SameAs(genericCategoryIdentificationCommand));
        }

        private CommandHandler CreateSut()
        {
            return new MyGenericCategoryIdentificationCommandHandler(_validatorMock.Object);
        }

        private IGenericCategoryIdentificationCommand<IGenericCategory> CreateGenericCategoryIdentificationCommand()
        {
            return CreateGenericCategoryIdentificationCommandMock().Object;
        }

        private Mock<IGenericCategoryIdentificationCommand<IGenericCategory>> CreateGenericCategoryIdentificationCommandMock()
        {
            return new Mock<IGenericCategoryIdentificationCommand<IGenericCategory>>();
        }

        private class MyGenericCategoryIdentificationCommandHandler : CommandHandler
        {
            #region Constructor

            public MyGenericCategoryIdentificationCommandHandler(IValidator validator) 
                : base(validator)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public IGenericCategoryIdentificationCommand<IGenericCategory> ManageRepositoryAsyncWasCalledWithCommand { get; private set; }

            #endregion

            #region Methods

            protected override Task<IGenericCategory> GetGenericCategoryAsync(int number) => throw new NotSupportedException();

            protected override Task ManageRepositoryAsync(IGenericCategoryIdentificationCommand<IGenericCategory> command)
            {
                NullGuard.NotNull(command, nameof(command));

                ManageRepositoryAsyncWasCalled = true;
                ManageRepositoryAsyncWasCalledWithCommand = command;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}