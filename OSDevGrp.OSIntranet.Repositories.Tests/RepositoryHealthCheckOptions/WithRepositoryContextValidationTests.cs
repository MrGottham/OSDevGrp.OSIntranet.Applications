using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithRepositoryContextValidationTests
    {
        [Test]
        [Category("UnitTest")]
        public void WithRepositoryContextValidation_WhenCalled_ReturnsNotNull()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithRepositoryContextValidation();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptions()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithRepositoryContextValidation();

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptionsWhereValidateRepositoryContextIsTrue()
        {
            Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            Repositories.RepositoryHealthCheckOptions result = sut.WithRepositoryContextValidation();

            Assert.That(result.ValidateRepositoryContext, Is.True);
        }

        private Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}