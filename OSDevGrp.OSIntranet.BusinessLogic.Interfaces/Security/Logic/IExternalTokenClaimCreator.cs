﻿using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
	public interface IExternalTokenClaimCreator
	{
		bool CanBuild(IDictionary<string, string> authenticationSessionItems);

		Claim Build(IDictionary<string, string> authenticationSessionItems, Func<string, string> protector);
	}
}