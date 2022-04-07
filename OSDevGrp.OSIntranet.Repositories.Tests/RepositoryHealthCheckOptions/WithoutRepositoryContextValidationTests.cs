using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithoutRepositoryContextValidationTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void WithoutRepositoryContextValidation_WhenCalled_ExpectValidateRepositoryContextIsFalse()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithoutRepositoryContextValidation();

            Assert.IsFalse(result.ValidateRepositoryContext);
        }

        private OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}