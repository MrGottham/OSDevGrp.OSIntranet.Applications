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
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithRepositoryContextValidation();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptions()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithRepositoryContextValidation();

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithRepositoryContextValidation_WhenCalled_ReturnsSameRepositoryHealthCheckOptionsWhereValidateRepositoryContextIsTrue()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithRepositoryContextValidation();

            Assert.IsTrue(result.ValidateRepositoryContext);
        }

        private OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}