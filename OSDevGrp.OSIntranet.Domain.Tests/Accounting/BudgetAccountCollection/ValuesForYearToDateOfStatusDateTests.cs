using System;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountCollection
{
    [TestFixture]
    public class ValuesForYearToDateOfStatusDateTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Customize<IBudgetAccountGroup>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountGroupMock().Object));
            _fixture.Customize<IBudgetAccount>(builder => builder.FromFactory(() => _fixture.BuildBudgetAccountMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForYearToDateOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IBudgetAccount>(_random.Next(5, 10)).ToArray());

            IBudgetInfoValues result = sut.ValuesForYearToDateOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForYearToDateOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsBudgetInfoValuesWhereBudgetIsEqualToZero()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IBudgetAccount>(_random.Next(5, 10)).ToArray());

            IBudgetInfoValues result = sut.ValuesForYearToDateOfStatusDate;

            Assert.That(result.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForYearToDateOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsBudgetInfoValuesWherePostedIsEqualToZero()
        {
            IBudgetAccountCollection sut = CreateSut();

            sut.Add(_fixture.CreateMany<IBudgetAccount>(_random.Next(5, 10)).ToArray());

            IBudgetInfoValues result = sut.ValuesForYearToDateOfStatusDate;

            Assert.That(result.Posted, Is.EqualTo(0M));
        }

        private IBudgetAccountCollection CreateSut()
        {
            return new Domain.Accounting.BudgetAccountCollection();
        }
    }
}