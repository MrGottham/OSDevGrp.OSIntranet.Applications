using System;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Queries.AccountingIdentificationQueryBase
{
    [TestFixture]
    public class StatusDateTests
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
        public void StatusDate_WhenGetterIsCalled_ExpectNoTime()
        {
            IAccountingIdentificationQuery sut = CreateSut();

            DateTime result = sut.StatusDate;

            Assert.That(result.TimeOfDay, Is.EqualTo(new TimeSpan(0, 0, 0, 0, 0)));
        }

        [Test]
        [Category("UnitTest")]
        public void StatusDate_WhenSetterIsCalled_ExpectNoTime()
        {
            DateTime statusDate = _fixture.Create<DateTime>().Date.AddMinutes(_fixture.Create<int>());
            IAccountingIdentificationQuery sut = CreateSut(statusDate);

            DateTime result = sut.StatusDate;

            Assert.That(result, Is.EqualTo(statusDate.Date));
        }

        private IAccountingIdentificationQuery CreateSut(DateTime? statusDate = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.StatusDate, statusDate ?? _fixture.Create<DateTime>())
                .Create();
        }

        private class Sut : BusinessLogic.Accounting.Queries.AccountingIdentificationQueryBase
        {
        }
    }
}