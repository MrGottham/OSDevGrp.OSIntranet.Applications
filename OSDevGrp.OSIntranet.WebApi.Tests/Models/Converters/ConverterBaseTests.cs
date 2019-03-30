using System.Collections.Generic;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Security;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Models.Converters
{
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
            IEnumerable<ConverterBase> sutCollection = _converterBaseTestHelper.GetConverters(typeof(AccessTokenModel).Assembly);

            string result = _converterBaseTestHelper.IsConfigurationValid(sutCollection);
            if (string.IsNullOrWhiteSpace(result))
            {
                return;
            }

            Assert.Fail(result);
        }
    }
}
