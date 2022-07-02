using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithoutRepositoryContextValidationTests
    {
        [Test]
        [Category("UnitTest")]
        public void WithoutRepositoryContextValidation_WhenCalled_ReturnsNotNull()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithoutRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptions()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithoutRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptionsWhereValidateRepositoryContextIsFalse()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.That(result.ValidateRepositoryContext, Is.False);
        }

        private OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}