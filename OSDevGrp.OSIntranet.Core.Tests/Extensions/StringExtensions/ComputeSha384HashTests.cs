using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Extensions;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Core.Tests.Extensions.StringExtensions
{
    [TestFixture]
    public class ComputeSha384HashTests
    {
        #region Private constants

        private static readonly Regex HexStringRegex = new("^[a-fA-F0-9]{96}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32));

        #endregion

        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => Core.Extensions.StringExtensions.ComputeSha384Hash(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsEmpty_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => string.Empty.ComputeSha384Hash());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsWhiteSpace_ThrowsArgumentNullException()
        {
            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => " ".ComputeSha384Hash());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            string result = _fixture.Create<string>().ComputeSha384Hash();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsNonEmptyString()
        {
            string result = _fixture.Create<string>().ComputeSha384Hash();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsNonEmptyHexString()
        {
            string result = _fixture.Create<string>().ComputeSha384Hash();

            Assert.That(HexStringRegex.IsMatch(result), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ComputeSha384Hash_WhenValueIsNotNullEmptyOrWhiteSpace_ReturnsNonEmptyHexStringMatchingComputedHasOfValue()
        {
            string value = _fixture.Create<string>();

            string result = value.ComputeSha384Hash();

            Assert.That(result, Is.EqualTo(ExpectedValue(value)));
        }

        private static string ExpectedValue(string value)
        {
            Core.NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            StringBuilder valueBuilder = new StringBuilder();
            foreach (byte b in Encoding.UTF8.GetBytes(value).ComputeSha384Hash())
            {
                valueBuilder.Append(b.ToString("x2"));
            }

            return valueBuilder.ToString();
        }
    }
}