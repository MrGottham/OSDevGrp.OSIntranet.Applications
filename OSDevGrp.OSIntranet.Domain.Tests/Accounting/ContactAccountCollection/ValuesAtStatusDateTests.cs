using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactAccountCollection
{
    [TestFixture]
    public class ValuesAtStatusDateTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<IContactAccount>(builder => builder.FromFactory(() => _fixture.BuildContactAccountMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IContactAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IContactAccount>(_random.Next(5, 10)).ToArray());

            IContactAccountCollectionValues result = sut.ValuesAtStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsContactAccountCollectionValuesWhereDebtorsIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IContactAccount>(_random.Next(5, 10)).ToArray());

            IContactAccountCollectionValues result = sut.ValuesAtStatusDate;

            Assert.That(result.Debtors, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsContactAccountCollectionValuesWhereCreditorsIsEqualToZero()
        {
            IContactAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IContactAccount>(_random.Next(5, 10)).ToArray());

            IContactAccountCollectionValues result = sut.ValuesAtStatusDate;

            Assert.That(result.Creditors, Is.EqualTo(0M));
        }

        private IContactAccountCollection CreateSut()
        {
            return new Domain.Accounting.ContactAccountCollection();
        }
    }
}