using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountCollection
{
    [TestFixture]
    public class ValuesAtEndOfLastYearFromStatusDateTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildAccountGroupMock().Object));
            _fixture.Customize<IAccount>(builder => builder.FromFactory(() => _fixture.BuildAccountMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IAccount>(_random.Next(5, 10)).ToArray());

            IAccountCollectionValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsAccountCollectionValuesWhereAssetsIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IAccount>(_random.Next(5, 10)).ToArray());

            IAccountCollectionValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result.Assets, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsAccountCollectionValuesWhereLiabilitiesIsEqualToZero()
        {
            IAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IAccount>(_random.Next(5, 10)).ToArray());

            IAccountCollectionValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result.Liabilities, Is.EqualTo(0M));
        }

        private IAccountCollection CreateSut()
        {
            return new Domain.Accounting.AccountCollection();
        }
    }
}