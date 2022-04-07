using System;
using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.ExternalData;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class ExternalDataMockBuilder
    {
        public static Mock<INews> BuildNewsMock(this Fixture fixture, string identifier = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Random random = new Random(fixture.Create<int>());

            Mock<INews> newsMock = new Mock<INews>();
            newsMock.Setup(m => m.Identifier)
                .Returns(identifier ?? fixture.Create<string>());
            newsMock.Setup(m => m.Timestamp)
                .Returns(DateTime.Now.AddDays(random.Next(0, 7) * -1).AddMinutes(random.Next(-120, 120)));
            newsMock.Setup(m => m.Header)
                .Returns(fixture.Create<string>());
            newsMock.Setup(m => m.Details)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            newsMock.Setup(m => m.Provider)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            newsMock.Setup(m => m.Author)
                .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
            newsMock.Setup(m => m.SourceUrl)
                .Returns(random.Next(100) > 50 ? new Uri($"https://localhost/{fixture.Create<string>()}") : null);
            newsMock.Setup(m => m.ImageUrl)
                .Returns(random.Next(100) > 50 ? new Uri($"https://localhost/{fixture.Create<string>()}.png") : null);
            return newsMock;
        }
    }
}