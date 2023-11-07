using NUnit.Framework;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    [TestFixture]
    public class UpdateLanguageAsyncTests : CommonRepositoryTestBase
    {
        [Test]
        [Category("UnitTest")]
        public void UpdateLanguageAsync_WhenLanguageIsNull_ThrowsArgumentNullException()
        {
            ICommonRepository sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateLanguageAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("language"));
            // ReSharper restore PossibleNullReferenceException
        }
    }
}