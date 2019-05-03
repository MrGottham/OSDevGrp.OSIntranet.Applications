using System;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.ObjectValidator
{
    [TestFixture]
    public class ShouldBeDeletableTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenIsDeletableGetterIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeDeletable<object, IDeletable>(_fixture.Create<object>(), null, GetType(), _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("deletableGetter"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValidatingTypeIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.BuildDeletableMock().Object), null, _fixture.Create<string>()));

            Assert.That(result.ParamName, Is.EqualTo("validatingType"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValidatingFieldIsNull_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() =>  _fixture.BuildDeletableMock().Object), GetType(), null));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValidatingFieldIsEmpty_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() =>  _fixture.BuildDeletableMock().Object), GetType(), string.Empty));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValidatingFieldIsWhiteSpace_ThrowsArgumentNullException()
        {
            IObjectValidator sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() =>  _fixture.BuildDeletableMock().Object), GetType(), " "));

            Assert.That(result.ParamName, Is.EqualTo("validatingField"));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNullAndNullIsAllowed_AssertDeletableGetterWasNotCalled()
        {
            IObjectValidator sut = CreateSut();

            bool deletableGetterWasCalled = false;
            sut.ShouldBeDeletable<object, IDeletable>(null, async obj => await Task.Run(() => 
                {
                    deletableGetterWasCalled = true;
                    return _fixture.BuildDeletableMock().Object;
                }),
                GetType(),
                _fixture.Create<string>(),
                true);

            Assert.That(deletableGetterWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNullAndNullIsAllowed_AssertDeletableWasNotCalledOnDeletable()
        {
            IObjectValidator sut = CreateSut();

            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock();
            sut.ShouldBeDeletable<object, IDeletable>(null, async obj => await Task.Run(() => deletableMock.Object), GetType(), _fixture.Create<string>(), true);

            deletableMock.Verify(m => m.Deletable, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNullAndNullIsAllowed_ReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            IValidator result = sut.ShouldBeDeletable<object, IDeletable>(null, async obj => await Task.Run(() =>  _fixture.BuildDeletableMock().Object), GetType(), _fixture.Create<string>(), true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNullAndNullIsNotAllowed_AssertDeletableGetterWasNotCalled()
        {
            IObjectValidator sut = CreateSut();

            bool deletableGetterWasCalled = false;
            Assert.Throws<IntranetValidationException>(() => sut.ShouldBeDeletable<object, IDeletable>(null, async obj => await Task.Run(() => 
                {
                    deletableGetterWasCalled = true;
                    return _fixture.BuildDeletableMock().Object;
                }),
                GetType(),
                _fixture.Create<string>()));

            Assert.That(deletableGetterWasCalled, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNullAndNullIsNotAllowed_AssertDeletableWasNotCalledOnDeletable()
        {
            IObjectValidator sut = CreateSut();

            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock();
            Assert.Throws<IntranetValidationException>(() => sut.ShouldBeDeletable<object, IDeletable>(null, async obj => await Task.Run(() => deletableMock.Object), GetType(), _fixture.Create<string>()));

            deletableMock.Verify(m => m.Deletable, Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNullAndNullIsNotAllowed_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeDeletable<object, IDeletable>(null, async obj => await Task.Run(() =>  _fixture.BuildDeletableMock().Object), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueCannotBeNull));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNotNull_AssertDeletableGetterWasCalled()
        {
            IObjectValidator sut = CreateSut();

            object value = _fixture.Create<object>();
            object deletableGetterWasCalledWithObject = null;
            sut.ShouldBeDeletable(value, async obj => await Task.Run(() =>
                {
                    deletableGetterWasCalledWithObject = obj;
                    return _fixture.BuildDeletableMock(true).Object;
                }),
                GetType(),
                _fixture.Create<string>());

            Assert.That(deletableGetterWasCalledWithObject, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNotNullAndDeletableGetterReturnsNull_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            IDeletable deletable = null;
            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() => deletable), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldReferToDeletableEntity));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNotNull_AssertDeletableWasCalledOnDeletable()
        {
            IObjectValidator sut = CreateSut();

            Mock<IDeletable> deletableMock = _fixture.BuildDeletableMock(true);
            sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() => deletableMock.Object), GetType(), _fixture.Create<string>());

            deletableMock.Verify(m => m.Deletable, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNotNullAndDeletable_ReturnsReturnsObjectValidator()
        {
            IObjectValidator sut = CreateSut();

            IValidator result = sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.BuildDeletableMock(true).Object), GetType(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.ObjectValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNotNullAndNotDeletable_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run(() => _fixture.BuildDeletableMock(false).Object), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldReferToDeletableEntity));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ShouldBeDeletable_WhenValueIsNotNullAndDeletableGetterThrowsException_ThrowsIntranetValidationException()
        {
            IObjectValidator sut = CreateSut();

            Exception exception = _fixture.Create<Exception>();
            IDeletable deletableGetter() => throw exception;

            Type validatingType = GetType();
            string validatingField = _fixture.Create<string>();
            IntranetValidationException result = Assert.Throws<IntranetValidationException>(() => sut.ShouldBeDeletable(_fixture.Create<object>(), async obj => await Task.Run((Func<IDeletable>) deletableGetter), validatingType, validatingField));

            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.ValueShouldReferToDeletableEntity));
            Assert.That(result.ValidatingType, Is.EqualTo(validatingType));
            Assert.That(result.ValidatingField, Is.EqualTo(validatingField));
            Assert.That(result.InnerException, Is.EqualTo(exception));
        }

        private IObjectValidator CreateSut()
        {
            return new BusinessLogic.Validation.ObjectValidator();
        }
   }
}