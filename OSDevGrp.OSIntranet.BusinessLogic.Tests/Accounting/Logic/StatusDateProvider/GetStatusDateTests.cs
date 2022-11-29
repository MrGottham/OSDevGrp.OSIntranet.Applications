using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.StatusDateProvider
{
    [TestFixture]
    public class GetStatusDateTests
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
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemException()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToNamedValueNotSetOnObject()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.NamedValueNotSetOnObject));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemExceptionWhereMessageIsNotNull()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemExceptionWhereMessageIsNotEmpty()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message, Is.Not.Empty);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemExceptionWhereMessageContainsStatusDate()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("'StatusDate'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemExceptionWhereMessageContainsStatusDateProvider()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.Contains("'StatusDateProvider'"), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasNotBeenSet_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            IStatusDateProvider sut = CreateSut();

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            IntranetSystemException result = Assert.Throws<IntranetSystemException>(() => sut.GetStatusDate());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void GetStatusDate_WhenStatusDateHasBeenSet_ReturnsStatusDate()
        {
            IStatusDateProvider sut = CreateSut();

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            ((IStatusDateSetter)sut).SetStatusDate(statusDate);

            DateTime result = sut.GetStatusDate();

            Assert.That(result, Is.EqualTo(statusDate));
        }

        private IStatusDateProvider CreateSut()
        {
            return new BusinessLogic.Accounting.Logic.StatusDateProvider();
        }
    }
}