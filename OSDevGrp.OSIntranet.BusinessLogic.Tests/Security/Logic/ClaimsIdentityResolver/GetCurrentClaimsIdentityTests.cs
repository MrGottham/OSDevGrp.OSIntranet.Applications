using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimsIdentityResolver
{
	[TestFixture]
	public class GetCurrentClaimsIdentityTests
	{
		#region Private variables

		private Mock<IPrincipalResolver> _principalResolverMock;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_principalResolverMock = new Mock<IPrincipalResolver>();
		}

		[Test]
		[Category("UnitTest")]
		public void GetCurrentClaimsIdentity_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
		{
			IClaimsIdentityResolver sut = CreateSut();

			sut.GetCurrentClaimsIdentity();

			_principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void GetCurrentClaimsIdentity_NoPrincipalWasReturned_ReturnsNull()
		{
			IClaimsIdentityResolver sut = CreateSut(false);

			ClaimsIdentity result = sut.GetCurrentClaimsIdentity();

			Assert.That(result, Is.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetCurrentClaimsIdentity_PrincipalWasReturned_ReturnsNotNull()
		{
			IClaimsIdentityResolver sut = CreateSut();

			ClaimsIdentity result = sut.GetCurrentClaimsIdentity();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		[Category("UnitTest")]
		public void GetCurrentClaimsIdentity_PrincipalWasReturned_ReturnsClaimsIdentityFromClaimsPrincipal()
		{
			ClaimsIdentity claimsIdentity = new ClaimsIdentity();
			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			IClaimsIdentityResolver sut = CreateSut(principal: claimsPrincipal);

			ClaimsIdentity result = sut.GetCurrentClaimsIdentity();

			Assert.That(result, Is.EqualTo(claimsIdentity));
		}

		private IClaimsIdentityResolver CreateSut(bool hasPrincipal = true, IPrincipal principal = null)
		{
			_principalResolverMock.Setup(m => m.GetCurrentPrincipal())
				.Returns(hasPrincipal ? principal ?? new ClaimsPrincipal(new ClaimsIdentity()) : null);

			return new BusinessLogic.Security.Logic.ClaimsIdentityResolver(_principalResolverMock.Object);
		}
	}
}