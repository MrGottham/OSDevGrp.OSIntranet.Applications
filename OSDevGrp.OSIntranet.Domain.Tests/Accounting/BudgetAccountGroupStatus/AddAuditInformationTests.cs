using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    [TestFixture]
    public class AddAuditInformationTests
    {
        #region Properties

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void AddAuditInformation_WhenCalled_ThrowsNotSupportedException()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            Assert.Throws<NotSupportedException>(() => sut.AddAuditInformation(DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>(), DateTime.UtcNow.AddDays(_random.Next(0, 365) * -1), _fixture.Create<string>()));
        }

        private IBudgetAccountGroupStatus CreateSut()
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}