using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithRepositoryContextValidationTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void WithRepositoryContextValidation_WhenCalled_ExpectValidateRepositoryContextIsTrue()
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