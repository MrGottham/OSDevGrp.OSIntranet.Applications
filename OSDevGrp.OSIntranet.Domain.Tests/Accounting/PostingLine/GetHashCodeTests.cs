using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class GetHashCodeTests
    {
        #region Private variables

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
        public void GetHashCode_WhenCalled_ReturnsHashCodeBasedOnIdentifier()
        {
            Guid identifier = Guid.NewGuid();
            IPostingLine sut = CreateSut(identifier);

            int result = sut.GetHashCode();

            Assert.That(result, Is.EqualTo(identifier.GetHashCode()));
        }

        private IPostingLine CreateSut(Guid identifier)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear);
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(identifier, new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock().Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, Math.Abs(_fixture.Create<int>()));
        }
    }
}