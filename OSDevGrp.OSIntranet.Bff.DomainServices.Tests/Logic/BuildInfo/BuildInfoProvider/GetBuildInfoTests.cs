using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.BuildInfo.BuildInfoProvider;

[TestFixture]
public class GetBuildInfoTests
{
    [Test]
    [Category("UnitTest")]
    public void GetBuildInfo_WhenCalled_ReturnsNotNull()
    {
        IBuildInfoProvider sut = CreateSut();

        IBuildInfo? result = sut.GetBuildInfo(GetAssembly());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetBuildInfo_WhenCalled_ReturnsBuildInfoWhereAssemblyIsEqualToAssemblyName()
    {
        IBuildInfoProvider sut = CreateSut();

        Assembly assembly = GetAssembly();
        IBuildInfo result = sut.GetBuildInfo(assembly);

        Assert.That(result.Assembly, Is.EqualTo(assembly.GetName().FullName));
    }

    [Test]
    [Category("UnitTest")]
    public void GetBuildInfo_WhenCalled_ReturnsBuildInfoWhereBuildTimeIsEqualToAssemblyCreationTime()
    {
        IBuildInfoProvider sut = CreateSut();

        Assembly assembly = GetAssembly();
        IBuildInfo result = sut.GetBuildInfo(assembly);

        Assert.That(result.BuildTime, Is.EqualTo(new DateTimeOffset(File.GetCreationTimeUtc(assembly.Location), TimeSpan.Zero)));
    }

    private static IBuildInfoProvider CreateSut()
    {
        return new DomainServices.Logic.BuildInfo.BuildInfoProvider();
    }

    private static Assembly GetAssembly()
    {
        return typeof(GetBuildInfoTests).Assembly;
    }
}