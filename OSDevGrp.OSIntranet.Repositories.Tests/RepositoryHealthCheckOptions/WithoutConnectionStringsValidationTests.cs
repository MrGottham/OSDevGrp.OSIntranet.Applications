using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithoutConnectionStringsValidationTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void WithoutConnectionStringsValidation_WhenCalled_ExpectValidateConnectionStringsIsFalse()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithoutConnectionStringsValidation();

            Assert.IsFalse(result.ValidateConnectionStrings);
        }

        private OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}