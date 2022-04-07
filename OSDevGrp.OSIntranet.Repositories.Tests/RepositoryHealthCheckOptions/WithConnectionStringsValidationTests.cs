using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Repositories.Tests.RepositoryHealthCheckOptions
{
    [TestFixture]
    public class WithConnectionStringsValidationTests
    {
        [Test]
        [Category("IntegrationTest")]
        public void WithConnectionStringsValidation_WhenCalled_ExpectValidateConnectionStringsIsTrue()
        {
            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions sut = CreateSut();

            OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions result = sut.WithConnectionStringsValidation();

            Assert.IsTrue(result.ValidateConnectionStrings);
        }

        private OSDevGrp.OSIntranet.Repositories.RepositoryHealthCheckOptions CreateSut()
        {
            return new Repositories.RepositoryHealthCheckOptions();
        }
    }
}