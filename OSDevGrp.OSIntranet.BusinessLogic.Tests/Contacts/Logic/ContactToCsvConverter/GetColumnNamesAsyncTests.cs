using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Contacts.Logic.ContactToCsvConverter
{
    [TestFixture]
    public class GetColumnNamesAsyncTests
    {
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
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNotNull()
        {
            IContactToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.GetColumnNamesAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollection()
        {
            IContactToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.GetColumnNamesAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollectionWithColumnNames()
        {
            IContactToCsvConverter sut = CreateSut();

            string[] result = (await sut.GetColumnNamesAsync()).ToArray();

            Assert.That(result.Length, Is.EqualTo(31));
            Assert.That(result[0], Is.EqualTo("Fulde navn"));
            Assert.That(result[1], Is.EqualTo("Fornavn"));
            Assert.That(result[2], Is.EqualTo("Mellemnavn(e)"));
            Assert.That(result[3], Is.EqualTo("Efternavn/Firmanavn"));
            Assert.That(result[4], Is.EqualTo("Adresse (linje 1)"));
            Assert.That(result[5], Is.EqualTo("Adresse (linje 2)"));
            Assert.That(result[6], Is.EqualTo("Postnr."));
            Assert.That(result[7], Is.EqualTo("By"));
            Assert.That(result[8], Is.EqualTo("Stat"));
            Assert.That(result[9], Is.EqualTo("Land"));
            Assert.That(result[10], Is.EqualTo("Primær tlf.nr./Mobil"));
            Assert.That(result[11], Is.EqualTo("Sekundær tlf.nr./Hjem"));
            Assert.That(result[12], Is.EqualTo("Mailadresse"));
            Assert.That(result[13], Is.EqualTo("Webside"));
            Assert.That(result[14], Is.EqualTo("Fødselsdato"));
            Assert.That(result[15], Is.EqualTo("Kontaktgruppe, nr."));
            Assert.That(result[16], Is.EqualTo("Kontaktgruppe, navn"));
            Assert.That(result[17], Is.EqualTo("Bekendtskab"));
            Assert.That(result[18], Is.EqualTo("Udlånsfrist"));
            Assert.That(result[19], Is.EqualTo("Betalingsbetingelse, nr."));
            Assert.That(result[20], Is.EqualTo("Betalingsbetingelse, navn"));
            Assert.That(result[21], Is.EqualTo("Firma, navn"));
            Assert.That(result[22], Is.EqualTo("Firma, adresse (linje 1)"));
            Assert.That(result[23], Is.EqualTo("Firma, adresse (linje 2)"));
            Assert.That(result[24], Is.EqualTo("Firma, postnr."));
            Assert.That(result[25], Is.EqualTo("Firma, by"));
            Assert.That(result[26], Is.EqualTo("Firma, stat"));
            Assert.That(result[27], Is.EqualTo("Firma, land"));
            Assert.That(result[28], Is.EqualTo("Firma, primær tlf.nr."));
            Assert.That(result[29], Is.EqualTo("Firma, sekundær tlf.nr."));
            Assert.That(result[30], Is.EqualTo("Firma, webside"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromConvertAsync()
        {
            IContactToCsvConverter sut = CreateSut();

            string[] result = (await sut.GetColumnNamesAsync()).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.ConvertAsync(_fixture.BuildContactMock().Object)).Count()));
        }

        private IContactToCsvConverter CreateSut()
        {
            return new BusinessLogic.Contacts.Logic.ContactToCsvConverter();
        }
    }
}