using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Configuration;

[TestFixture]
public class JsonSerializerSettingsTests : ServiceGatewayTestBase
{
    #region Private variables

    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    #region Methods

    [Test]
    [Category("UnitTest")]
    public void UpdateJsonSerializerSettings_WhenCalled_ConfiguresDefaultIgnoreConditionToWhenWritingNull()
    {
        JsonSerializerOptions settings = new JsonSerializerOptions();

        settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        Assert.That(settings.DefaultIgnoreCondition, Is.EqualTo(JsonIgnoreCondition.WhenWritingNull));
    }

    [Test]
    [Category("UnitTest")]
    public void UpdateJsonSerializerSettings_WhenSerializingObjectWithNullProperties_ExcludesNullPropertiesFromJson()
    {
        JsonSerializerOptions settings = new JsonSerializerOptions();
        settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        TestModel testModel = new TestModel
        {
            Id = _fixture!.Create<int>(),
            Name = _fixture!.Create<string>(),
            Description = null,
            Value = null
        };

        string json = JsonSerializer.Serialize(testModel, settings);

        Assert.That(json, Does.Not.Contain("\"Description\""));
        Assert.That(json, Does.Not.Contain("\"Value\""));
        Assert.That(json, Does.Contain("\"Id\""));
        Assert.That(json, Does.Contain("\"Name\""));
    }

    [Test]
    [Category("UnitTest")]
    public void UpdateJsonSerializerSettings_WhenSerializingObjectWithAllNullProperties_ExcludesAllNullPropertiesFromJson()
    {
        JsonSerializerOptions settings = new JsonSerializerOptions();
        settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        TestModel testModel = new TestModel
        {
            Id = 0,
            Name = null,
            Description = null,
            Value = null
        };

        string json = JsonSerializer.Serialize(testModel, settings);

        Assert.That(json, Does.Not.Contain("\"Name\""));
        Assert.That(json, Does.Not.Contain("\"Description\""));
        Assert.That(json, Does.Not.Contain("\"Value\""));
        Assert.That(json, Does.Contain("\"Id\""));
    }

    [Test]
    [Category("UnitTest")]
    public void UpdateJsonSerializerSettings_WhenSerializingObjectWithNoNullProperties_IncludesAllPropertiesInJson()
    {
        JsonSerializerOptions settings = new JsonSerializerOptions();
        settings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        TestModel testModel = new TestModel
        {
            Id = _fixture!.Create<int>(),
            Name = _fixture!.Create<string>(),
            Description = _fixture!.Create<string>(),
            Value = _fixture!.Create<string>()
        };

        string json = JsonSerializer.Serialize(testModel, settings);

        Assert.That(json, Does.Contain("\"Id\""));
        Assert.That(json, Does.Contain("\"Name\""));
        Assert.That(json, Does.Contain("\"Description\""));
        Assert.That(json, Does.Contain("\"Value\""));
    }

    #endregion

    #region Nested classes

    private class TestModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Value { get; set; }
    }

    #endregion
}