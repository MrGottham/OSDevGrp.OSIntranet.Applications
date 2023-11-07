using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Threading.Tasks;
using CommandHandler = OSDevGrp.OSIntranet.BusinessLogic.Core.CommandHandlers.GenericCategoryDataCommandHandlerBase<OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands.IGenericCategoryDataCommand<OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>, OSDevGrp.OSIntranet.Domain.Interfaces.Core.IGenericCategory>;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Core.CommandHandlers.GenericCategoryDataCommandHandlerBase
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
        public async Task ExecuteAsync_WhenCalled_AssertValidateWasCalledOnGenericCategoryDataCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IGenericCategoryDataCommand<IGenericCategory>> genericCategoryDataCommandMock = CreateGenericCategoryDataCommandMock();
            await sut.ExecuteAsync(genericCategoryDataCommandMock.Object);

            genericCategoryDataCommandMock.Verify(m => m.Validate(
                    It.Is<IValidator>(value => value != null && value == _validatorMock.Object),
                    It.IsNotNull<Func<bool>>(),
					It.IsNotNull<Func<int, Task<IGenericCategory>>>()),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertToDomainWasCalledOnGenericCategoryDataCommand()
        {
            CommandHandler sut = CreateSut();

            Mock<IGenericCategoryDataCommand<IGenericCategory>> genericCategoryDataCommandMock = CreateGenericCategoryDataCommandMock();
            await sut.ExecuteAsync(genericCategoryDataCommandMock.Object);

            genericCategoryDataCommandMock.Verify(m => m.ToDomain(), Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnGenericCategoryDataCommandHandlerBase()
        {
            CommandHandler sut = CreateSut();

            await sut.ExecuteAsync(CreateGenericCategoryDataCommand());

            Assert.That(((MyGenericCategoryDataCommandHandler)sut).ManageRepositoryAsyncWasCalled, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ExecuteAsync_WhenCalled_AssertManageRepositoryAsyncWasCalledOnGenericCategoryDataCommandHandlerBaseWithSameGenericCategoryFromGenericCategoryDataCommand()
        {
            CommandHandler sut = CreateSut();

            IGenericCategory genericCategory = new Mock<IGenericCategory>().Object;
            await sut.ExecuteAsync(CreateGenericCategoryDataCommand(genericCategory));

            Assert.That(((MyGenericCategoryDataCommandHandler)sut).ManageRepositoryAsyncWasCalledWithGenericCategory, Is.SameAs(genericCategory));
        }

        private CommandHandler CreateSut()
        {
            return new MyGenericCategoryDataCommandHandler(_validatorMock.Object);
        }

        private IGenericCategoryDataCommand<IGenericCategory> CreateGenericCategoryDataCommand(IGenericCategory genericCategory = null)
        {
            return CreateGenericCategoryDataCommandMock(genericCategory).Object;
        }

        private Mock<IGenericCategoryDataCommand<IGenericCategory>> CreateGenericCategoryDataCommandMock(IGenericCategory genericCategory = null)
        {
            Mock<IGenericCategoryDataCommand<IGenericCategory>> genericCategoryDataCommandMock = new Mock<IGenericCategoryDataCommand<IGenericCategory>>();
            genericCategoryDataCommandMock.Setup(m => m.ToDomain())
                .Returns(genericCategory ?? new Mock<IGenericCategory>().Object);
            return genericCategoryDataCommandMock;
        }

        private class MyGenericCategoryDataCommandHandler : CommandHandler
        {
            #region Constructor

            public MyGenericCategoryDataCommandHandler(IValidator validator)
                : base(validator)
            {
            }

            #endregion

            #region Properties

            public bool ManageRepositoryAsyncWasCalled { get; private set; }

            public IGenericCategory ManageRepositoryAsyncWasCalledWithGenericCategory { get; private set; }

            #endregion

            #region Methods

            protected override Task<IGenericCategory> GetGenericCategoryAsync(int number) => throw new NotSupportedException();

            protected override Task ManageRepositoryAsync(IGenericCategory genericCategory)
            {
                NullGuard.NotNull(genericCategory, nameof(genericCategory));

                ManageRepositoryAsyncWasCalled = true;
                ManageRepositoryAsyncWasCalledWithGenericCategory = genericCategory;

                return Task.CompletedTask;
            }

            #endregion
        }
    }
}
