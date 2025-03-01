using System.Security.Claims;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.ServiceCollectionExtensions;

[TestFixture]
public class AddDomainServicesTests : DomainServicesTestBase
{
    [Test]
    [Category("IntegrationTest")]
    public void AddDomainServices_WhenCalled_ExpectAllCommandFeaturesCanBeResolved()
    {
        using FeatureSetupTester featureSetupTester = new FeatureSetupTester(CreateTestConfiguration());

        foreach (Type requestType in featureSetupTester.GetRequestTypes(typeof(DomainServices.ServiceCollectionExtensions).Assembly).Where(featureSetupTester.HasCommandFeature))
        {
            try
            {
                object _ = featureSetupTester.GetCommandFeature(requestType);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }

    [Test]
    [Category("IntegrationTest")]
    public void AddDomainServices_WhenCalled_ExpectAllQueryFeaturesCanBeResolved()
    {
        using FeatureSetupTester featureSetupTester = new FeatureSetupTester(CreateTestConfiguration());

        foreach (Type requestType in featureSetupTester.GetRequestTypes(typeof(DomainServices.ServiceCollectionExtensions).Assembly).Where(featureSetupTester.HasQueryFeature))
        {
            try
            {
                object _ = featureSetupTester.GetQueryFeature(requestType);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}