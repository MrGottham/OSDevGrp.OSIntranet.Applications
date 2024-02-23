using AutoFixture;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ExternalTokenClaimCreator
{
	public abstract class ExternalTokenClaimCreatorBase
	{
		#region Methods

		protected static IDictionary<string, string> CreateAuthenticationSessionItems(Fixture fixture, bool hasExternalTokenClaimType = true, string externalTokenClaimType = null)
		{
			NullGuard.NotNull(fixture, nameof(fixture));

			IDictionary<string, string> authenticationSessionItems = new ConcurrentDictionary<string, string>();

			if (hasExternalTokenClaimType)
			{
				authenticationSessionItems.Add(AuthenticationSessionKeys.ExternalTokenClaimTypeKey, externalTokenClaimType ?? fixture.Create<string>());
			}

			return authenticationSessionItems;
		}

		#endregion
	}
}