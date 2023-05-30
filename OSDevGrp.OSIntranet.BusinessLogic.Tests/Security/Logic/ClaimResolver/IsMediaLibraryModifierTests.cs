using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimResolver
{
	[TestFixture]
	public class IsMediaLibraryModifierTests
	{
		#region Private variables

		private Mock<IPrincipalResolver> _principalResolverMock;
		private Fixture _fixture;

		#endregion

		[SetUp]
		public void SetUp()
		{
			_principalResolverMock = new Mock<IPrincipalResolver>();
			_fixture = new Fixture();
		}

		[Test]
		[Category("UnitTest")]
		public void IsMediaLibraryModifier_WhenCalled_AssertGetCurrentPrincipalWasCalledOnPrincipalResolver()
		{
			IClaimResolver sut = CreateSut();

			sut.IsMediaLibraryModifier();

			_principalResolverMock.Verify(m => m.GetCurrentPrincipal(), Times.Once);
		}

		[Test]
		[Category("UnitTest")]
		public void IsMediaLibraryModifier_WhenCalledAndPrincipalDoesNotHaveMediaLibraryModifierClaim_ReturnsFalse()
		{
			IPrincipal principal = CreateClaimsPrincipal(new[] { new Claim(_fixture.Create<string>(), _fixture.Create<string>()) });
			IClaimResolver sut = CreateSut(principal);

			bool result = sut.IsMediaLibraryModifier();

			Assert.That(result, Is.False);
		}

		[Test]
		[Category("UnitTest")]
		public void IsMediaLibraryModifier_WhenCalledAndPrincipalHasMediaLibraryModifierClaim_ReturnsTrue()
		{
			IPrincipal principal = CreateClaimsPrincipal(new[] { new Claim(_fixture.Create<string>(), _fixture.Create<string>()), ClaimHelper.CreateMediaLibraryModifierClaim() });
			IClaimResolver sut = CreateSut(principal);

			bool result = sut.IsMediaLibraryModifier();

			Assert.That(result, Is.True);
		}

		private IClaimResolver CreateSut(IPrincipal principal = null)
		{
			_principalResolverMock.Setup(m => m.GetCurrentPrincipal())
				.Returns(principal ?? CreateClaimsPrincipal());

			return new BusinessLogic.Security.Logic.ClaimResolver(_principalResolverMock.Object);
		}

		private IPrincipal CreateClaimsPrincipal(IEnumerable<Claim> claimCollection = null)
		{
			return new ClaimsPrincipal(new ClaimsIdentity(claimCollection ?? new Claim[] { }));
		}
	}
}