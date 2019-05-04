using System;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class CommonMockBuilder
    {
        public static Mock<ILetterHead> BuildLetterHeadMock(this Fixture fixture, int? number = null, string name = null, string line1 = null, string line2 = null, string line3 = null, string line4 = null, string line5 = null, string line6 = null, string line7 = null, string companyIdentificationNumber = null, bool? isDeletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<ILetterHead> letterHeadMock = new Mock<ILetterHead>();
            letterHeadMock.Setup(m => m.Number)
                .Returns(number ?? fixture.Create<int>());
            letterHeadMock.Setup(m => m.Name)
                .Returns(name ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line1)
                .Returns(line1 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line2)
                .Returns(line2 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line3)
                .Returns(line3 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line4)
                .Returns(line4 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line5)
                .Returns(line5 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line6)
                .Returns(line6 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Line7)
                .Returns(line7 ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.CompanyIdentificationNumber)
                .Returns(companyIdentificationNumber ?? fixture.Create<string>());
            letterHeadMock.Setup(m => m.Deletable)
                .Returns(isDeletable ?? fixture.Create<bool>());
            letterHeadMock.Setup(m => m.CreatedDateTime)
                .Returns(fixture.Create<DateTime>());
            letterHeadMock.Setup(m => m.CreatedByIdentifier)
                .Returns(fixture.Create<string>());
            letterHeadMock.Setup(m => m.ModifiedDateTime)
                .Returns(fixture.Create<DateTime>());
            letterHeadMock.Setup(m => m.ModifiedByIdentifier)
                .Returns(fixture.Create<string>());
            return letterHeadMock;
        }
    }
}