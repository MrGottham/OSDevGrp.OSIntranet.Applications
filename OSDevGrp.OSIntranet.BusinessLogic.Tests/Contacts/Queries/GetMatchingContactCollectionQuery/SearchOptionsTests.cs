using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Queries.GetMatchingContactCollectionQuery
{
    [TestFixture]
    public class SearchOptionsTests
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
        public void SearchOptions_WhenSearchWithinNameIsTrue_ReturnSearchOptionsWithFlagForName()
        {
            const bool searchWithinName = true;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinName);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.Name), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinNameIsFalse_ReturnSearchOptionsWithoutFlagForName()
        {
            const bool searchWithinName = false;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinName);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.Name), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinMailAddressIsTrue_ReturnSearchOptionsWithFlagForMailAddress()
        {
            const bool searchWithinMailAddress = true;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinMailAddress: searchWithinMailAddress);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.MailAddress), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinMailAddressIsFalse_ReturnSearchOptionsWithoutFlagForMailAddress()
        {
            const bool searchWithinMailAddress = false;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinMailAddress: searchWithinMailAddress);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.MailAddress), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinPrimaryPhoneIsTrue_ReturnSearchOptionsWithFlagForPrimaryPhone()
        {
            const bool searchWithinPrimaryPhone = true;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinPrimaryPhone: searchWithinPrimaryPhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.PrimaryPhone), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinPrimaryPhoneIsFalse_ReturnSearchOptionsWithoutFlagForPrimaryPhone()
        {
            const bool searchWithinPrimaryPhone = false;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinPrimaryPhone: searchWithinPrimaryPhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.PrimaryPhone), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinSecondaryPhoneIsTrue_ReturnSearchOptionsWithFlagForSecondaryPhone()
        {
            const bool searchWithinSecondaryPhone = true;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinSecondaryPhone: searchWithinSecondaryPhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.SecondaryPhone), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinSecondaryPhoneIsFalse_ReturnSearchOptionsWithoutFlagForSecondaryPhone()
        {
            const bool searchWithinSecondaryPhone = false;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinSecondaryPhone: searchWithinSecondaryPhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.SecondaryPhone), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinHomePhoneIsTrue_ReturnSearchOptionsWithFlagForHomePhone()
        {
            const bool searchWithinHomePhone = true;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinHomePhone: searchWithinHomePhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.HomePhone), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinHomePhoneIsFalse_ReturnSearchOptionsWithoutFlagForHomePhone()
        {
            const bool searchWithinHomePhone = false;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinHomePhone: searchWithinHomePhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.HomePhone), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinMobilePhoneIsTrue_ReturnSearchOptionsWithFlagForMobilePhone()
        {
            const bool searchWithinMobilePhone = true;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinMobilePhone: searchWithinMobilePhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.MobilePhone), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void SearchOptions_WhenSearchWithinMobilePhoneIsFalse_ReturnSearchOptionsWithoutFlagForMobilePhone()
        {
            const bool searchWithinMobilePhone = false;
            IGetMatchingContactCollectionQuery sut = CreateSut(searchWithinMobilePhone: searchWithinMobilePhone);

            SearchOptions result = sut.SearchOptions;

            Assert.That(result.HasFlag(SearchOptions.MobilePhone), Is.False);
        }

        private IGetMatchingContactCollectionQuery CreateSut(bool? searchWithinName = null, bool? searchWithinMailAddress = null, bool? searchWithinPrimaryPhone = null, bool? searchWithinSecondaryPhone = null, bool? searchWithinHomePhone = null, bool? searchWithinMobilePhone = null)
        {
            return _fixture.Build<BusinessLogic.Contacts.Queries.GetMatchingContactCollectionQuery>()
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