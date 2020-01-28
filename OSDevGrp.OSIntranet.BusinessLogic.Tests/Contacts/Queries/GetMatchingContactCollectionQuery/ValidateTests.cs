using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Queries.GetMatchingContactCollectionQuery
{
    [TestFixture]
    public class ValidateTests
    {
        #region Private variables

        private ValidatorMockContext _validatorMockContext;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _validatorMockContext = new ValidatorMockContext();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            IGetMatchingContactCollectionQuery sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Validate(null));

            Assert.That(result.ParamName, Is.EqualTo("validator"));
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldNotBeNullOrWhiteSpaceWasCalledOnStringValidatorForSearchFor()
        {
            string searchFor = _fixture.Create<string>();
            IGetMatchingContactCollectionQuery sut = CreateSut(searchFor);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            _validatorMockContext.StringValidatorMock.Verify(m => m.ShouldNotBeNullOrWhiteSpace(
                    It.Is<string>(value => string.CompareOrdinal(value, searchFor) == 0),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "SearchFor") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_AssertShouldBeGreaterThanZeroWasCalledOnIntegerValidatorForSearchOptions()
        {
            bool searchWithinName = _fixture.Create<bool>();
            bool searchWithinMailAddress = _fixture.Create<bool>();
            bool searchWithinPrimaryPhone = _fixture.Create<bool>();
            bool searchWithinSecondaryPhone = _fixture.Create<bool>();
            bool searchWithinHomePhone = _fixture.Create<bool>();
            bool searchWithinMobilePhone = _fixture.Create<bool>();
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinName: searchWithinName, searchWithinMailAddress: searchWithinMailAddress, searchWithinPrimaryPhone: searchWithinPrimaryPhone, searchWithinSecondaryPhone: searchWithinSecondaryPhone, searchWithinHomePhone: searchWithinHomePhone, searchWithinMobilePhone: searchWithinMobilePhone);

            sut.Validate(_validatorMockContext.ValidatorMock.Object);

            int searchOptions = (searchWithinName ? (int) SearchOptions.Name : 0) +
                                (searchWithinMailAddress ? (int) SearchOptions.MailAddress : 0) +
                                (searchWithinPrimaryPhone ? (int) SearchOptions.PrimaryPhone : 0) +
                                (searchWithinSecondaryPhone ? (int) SearchOptions.SecondaryPhone : 0) +
                                (searchWithinHomePhone ? (int) SearchOptions.HomePhone : 0) +
                                (searchWithinMobilePhone ? (int) SearchOptions.MobilePhone : 0);

            _validatorMockContext.IntegerValidatorMock.Verify(m => m.ShouldBeGreaterThanZero(
                    It.Is<int>(value => value == searchOptions),
                    It.Is<Type>(value => value == sut.GetType()),
                    It.Is<string>(value => string.CompareOrdinal(value, "SearchOptions") == 0)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Validate_WhenCalled_ReturnsValidator()
        {
            IGetMatchingContactCollectionQuery sut = CreateSut();

            IValidator result = sut.Validate(_validatorMockContext.ValidatorMock.Object);

            Assert.That(result, Is.EqualTo(_validatorMockContext.ValidatorMock.Object));
        }

        private IGetMatchingContactCollectionQuery CreateSut(string searchFor = null, bool? searchWithinName = null, bool? searchWithinMailAddress = null, bool? searchWithinPrimaryPhone = null, bool? searchWithinSecondaryPhone = null, bool? searchWithinHomePhone = null, bool? searchWithinMobilePhone = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Queries.GetMatchingContactCollectionQuery>()
                .With(m => m.SearchFor, searchFor ?? _fixture.Create<string>())
                .With(m => m.SearchWithinName, searchWithinName ?? _fixture.Create<bool>())
                .With(m => m.SearchWithinMailAddress, searchWithinMailAddress ?? _fixture.Create<bool>())
                .With(m => m.SearchWithinPrimaryPhone, searchWithinPrimaryPhone ?? _fixture.Create<bool>())
                .With(m => m.SearchWithinSecondaryPhone, searchWithinSecondaryPhone ?? _fixture.Create<bool>())
                .With(m => m.SearchWithinHomePhone, searchWithinHomePhone ?? _fixture.Create<bool>())
                .With(m => m.SearchWithinMobilePhone, searchWithinMobilePhone ?? _fixture.Create<bool>())
                .Create();
        }
    }
}