using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation.PermissionValidator
{
    [TestFixture]
    public class HasNecessaryPermissionTests
    {
        [Test]
        [Category("UnitTest")]
        public void HasNecessaryPermission_WhenNecessaryPermissionGrantedIsTrue_ReturnsNotNull()
        {
            IPermissionValidator sut = CreateSut();

            IValidator result = sut.HasNecessaryPermission(true);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void HasNecessaryPermission_WhenNecessaryPermissionGrantedIsTrue_ReturnsPermissionValidator()
        {
            IPermissionValidator sut = CreateSut();

            IValidator result = sut.HasNecessaryPermission(true);

            Assert.That(result, Is.TypeOf<BusinessLogic.Validation.PermissionValidator>());
        }

        [Test]
        [Category("UnitTest")]
        public void HasNecessaryPermission_WhenNecessaryPermissionGrantedIsTrue_ReturnsSamePermissionValidator()
        {
            IPermissionValidator sut = CreateSut();

            IValidator result = sut.HasNecessaryPermission(true);

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void HasNecessaryPermission_WhenNecessaryPermissionGrantedIsFalse_ThrowIntranetBusinessException()
        {
            IPermissionValidator sut = CreateSut();

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.HasNecessaryPermission(false));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void HasNecessaryPermission_WhenNecessaryPermissionGrantedIsFalse_ThrowIntranetBusinessExceptionWhereErrorCodeIsEqualToMissingNecessaryPermission()
        {
            IPermissionValidator sut = CreateSut();

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.HasNecessaryPermission(false));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.MissingNecessaryPermission));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void HasNecessaryPermission_WhenNecessaryPermissionGrantedIsFalse_ThrowIntranetBusinessExceptionWhereInnerExceptionIsNull()
        {
            IPermissionValidator sut = CreateSut();

            IntranetBusinessException result = Assert.Throws<IntranetBusinessException>(() => sut.HasNecessaryPermission(false));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        private IPermissionValidator CreateSut()
        {
            return new BusinessLogic.Validation.PermissionValidator();
        }
    }
}