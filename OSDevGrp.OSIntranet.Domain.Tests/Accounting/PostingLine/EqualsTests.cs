using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.PostingLine
{
    [TestFixture]
    public class EqualsTests
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
        public void Equals_WhenObjectIsNull_ReturnsFalse()
        {
            IPostingLine sut = CreateSut();

            bool result = sut.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsNonPostingLine_ReturnsFalse()
        {
            IPostingLine sut = CreateSut();

            bool result = sut.Equals(_fixture.Create<object>());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsPostingLineWhereIdentifierDoesNotMatch_ReturnsFalse()
        {
            IPostingLine sut = CreateSut();

            bool result = sut.Equals(CreateSut());

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Equals_WhenObjectIsPostingLineWhereIdentifierDoesMatch_ReturnsTrue()
        {
            Guid identifier = Guid.NewGuid();
            IPostingLine sut = CreateSut(identifier);

            bool result = sut.Equals(CreateSut(identifier));

            Assert.That(result, Is.True);
        }

        private IPostingLine CreateSut(Guid? identifier = null)
        {
            int year = _random.Next(InfoBase<ICreditInfo>.MinYear, InfoBase<ICreditInfo>.MaxYear);
            int month = _random.Next(InfoBase<ICreditInfo>.MinMonth, InfoBase<ICreditInfo>.MaxMonth);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month));

            return new Domain.Accounting.PostingLine(identifier ?? Guid.NewGuid(), new DateTime(year, month, day), _fixture.Create<string>(), _fixture.BuildAccountMock().Object, _fixture.Create<string>(), null, Math.Abs(_fixture.Create<decimal>()), Math.Abs(_fixture.Create<decimal>()), null, Math.Abs(_fixture.Create<int>()));
        }
    }
}