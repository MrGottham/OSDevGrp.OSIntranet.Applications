using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Commands.CreateContactGroupCommand
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Mock<IContactRepository> _contactRepositoryMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            ICreateContactGroupCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null, _contactRepositoryMock.Object));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenContactRepositoryIsNull_ThrowsArgumentNullException()
        {
            ICreateContactGroupCommand sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(_validatorMockContext.ValidatorMock.Object, null));

            Assert.That(result.ParamName, Is.EqualTo("contactRepository"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeUnknownValueWasCalledOnObjectValidator()
        {
            int number = _fixture.Create<int>();
            ICreateContactGroupCommand sut = CreateSut(number);

            sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            _validatorMockContext.ObjectValidatorMock.Verify(m => m.ShouldBeUnknownValue(
                    It.Is<int>(value => value == number),
                    It.IsNotNull<Func<int, Task<bool>>>(),
                    It.Is<Type>(type => type == sut.GetType()),
                    It.Is<string>(field => string.CompareOrdinal(field, "Number") == 0),
                    It.Is<bool>(value => value == false)),
                Times.Once());
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            ICreateContactGroupCommand sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object, _contactRepositoryMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private ICreateContactGroupCommand CreateSut(int? number = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Commands.CreateContactGroupCommand>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .Create();
        }
    }
}
