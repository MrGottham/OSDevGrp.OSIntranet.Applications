using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters
{
    [TestFixture]
    public class ConverterBaseTests : RepositoryTestBase
    {
        #region Private variables

        private ConverterBaseTestHelper _converterBaseTestHelper;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _converterBaseTestHelper = new ConverterBaseTestHelper();
        }

        [Test]
        [Category("UnitTest")]
        public void ConverterBase_ForAllConvertersBasedOnConverterBase_AssertMapperConfigurationIsValid()
        {
            IEnumerable<ConverterBase> sutCollection = _converterBaseTestHelper.GetConverters(typeof(Repositories.AccountingRepository).Assembly, CreateLicensesOptions(), CreateLoggerFactory());

            string result = _converterBaseTestHelper.IsConfigurationValid(sutCollection);
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            Assert.Fail(result);
        }
    }
}