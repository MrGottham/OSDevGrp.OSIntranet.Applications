using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters
{
    [TestFixture]
    public class ConverterBaseTests
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
            IEnumerable<ConverterBase> sutCollection = _converterBaseTestHelper.GetConverters(typeof(Repositories.AccountingRepository).Assembly);

            string result = _converterBaseTestHelper.IsConfigurationValid(sutCollection);
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            Assert.Fail(result);
        }
    }
}