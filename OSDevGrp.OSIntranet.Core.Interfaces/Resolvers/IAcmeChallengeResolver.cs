namespace OSDevGrp.OSIntranet.Core.Interfaces.Resolvers
{
    public interface IAcmeChallengeResolver
    {
        string GetWellKnownChallengeToken();

        string GetConstructedKeyAuthorization(string challengeToken);
    }
}