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
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithoutRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptions()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithoutRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptionsWhereValidateRepositoryContextIsFalse()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.That(result.ValidateRepositoryContext, Is.False);
        }

        private Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}