using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.TestHelpers
{
    public static class CoreMockBuilder
    {
        public static Mock<IDeletable> BuildDeletableMock(this Fixture fixture, bool? isDeletable = null)
        {
            NullGuard.NotNull(fixture, nameof(fixture));

            Mock<IDeletable> deletableMock = new Mock<IDeletable>();
            deletableMock.Setup(m => m.Deletable)
                .Returns(isDeletable ?? fixture.Create<bool>());
            return deletableMock;
        }
    }
}