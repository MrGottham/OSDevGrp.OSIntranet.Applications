using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Commands;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Common.Commands.LetterHeadCommandBase
{
    [TestFixture]
    public class ToDomainTests
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
        public void ToDomain_WhenCalled_ReturnsLetterHead()
        {
            ILetterHeadCommand sut = CreateSut();

            ILetterHead result = sut.ToDomain();

            Assert.That(result, Is.TypeOf<LetterHead>());
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithNumberFromCommand()
        {
            int number = _fixture.Create<int>();
            ILetterHeadCommand sut = CreateSut(number);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Number, Is.EqualTo(number));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithNameFromCommand()
        {
            string name = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(name: name);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine1FromCommand()
        {
            string line1 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line1: line1);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line1, Is.EqualTo(line1));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine2FromCommand()
        {
            string line2 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line2: line2);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line2, Is.EqualTo(line2));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine3FromCommand()
        {
            string line3 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line3: line3);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line3, Is.EqualTo(line3));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine4FromCommand()
        {
            string line4 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line4: line4);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line4, Is.EqualTo(line4));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine5FromCommand()
        {
            string line5 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line5: line5);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line5, Is.EqualTo(line5));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine6FromCommand()
        {
            string line6 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line6: line6);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line6, Is.EqualTo(line6));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithLine7FromCommand()
        {
            string line7 = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(line7: line7);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.Line7, Is.EqualTo(line7));
        }

        [Test]
        [Category("UnitTest")]
        public void ToDomain_WhenCalled_ReturnsLetterHeadWithCompanyIdentificationNumberFromCommand()
        {
            string companyIdentificationNumber = _fixture.Create<string>();
            ILetterHeadCommand sut = CreateSut(companyIdentificationNumber: companyIdentificationNumber);

            ILetterHead result = sut.ToDomain();

            Assert.That(result.CompanyIdentificationNumber, Is.EqualTo(companyIdentificationNumber));
        }

        private ILetterHeadCommand CreateSut(int? number = null, string name = null, string line1 = null, string line2 = null, string line3 = null, string line4 = null, string line5 = null, string line6 = null, string line7 = null, string companyIdentificationNumber = null)
        {
            return _fixture.Build<Sut>()
                .With(m => m.Number, number ?? _fixture.Create<int>())
                .With(m => m.Name, name ?? _fixture.Create<string>())
                .With(m => m.Line1, line1 ?? _fixture.Create<string>())
                .With(m => m.Line2, line2 ?? _fixture.Create<string>())
                .With(m => m.Line3, line3 ?? _fixture.Create<string>())
                .With(m => m.Line4, line4 ?? _fixture.Create<string>())
                .With(m => m.Line5, line5 ?? _fixture.Create<string>())
                .With(m => m.Line6, line6 ?? _fixture.Create<string>())
                .With(m => m.Line7, line7 ?? _fixture.Create<string>())
                .With(m => m.CompanyIdentificationNumber, companyIdentificationNumber ?? _fixture.Create<string>())
                .Create();
        }

        private class Sut : BusinessLogic.Common.Commands.LetterHeadCommandBase
        {
        }
    }
}