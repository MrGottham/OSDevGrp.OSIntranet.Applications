# Diary: Upgrading the solution to .NET 11

The goal is to retarget the solution and its containers to .NET 11 while keeping Entity Framework Core 10 and the current Oracle MySQL provider until an EF Core 11-compatible provider is available.

## Step 1: Retargeting projects and aligning dependencies

**Author:** main

### Prompt Context

**Verbatim prompt:** "Lets start the implementation"
**Interpretation:** Begin the .NET 11 solution and container upgrade.
**Inferred intent:** Make the framework migration in small, verifiable steps without forcing an unsupported EF Core/MySQL provider upgrade.
**Scope selected:** "net11-keep-ef10"

### What I did

Confirmed the worktree was clean and the installed SDK was 11.0.100. The baseline solution build succeeded with zero warnings and errors. The filtered unit-test run produced passing results for the Core, BFF ServiceGateways, Repositories, WebApi, and MVC test projects, but the Domain and BusinessLogic test hosts remained active for over eight minutes without reporting results; I stopped the run.

Retargeted the project files to `net11.0`, updated the .NET framework package references to 11.0.0 while retaining EF Core Tools and the EF health-check integration package at 10.0.14, and kept the MySQL provider at 10.0.12. Updated the BFF's direct `Microsoft.OpenApi` reference to 3.11.0 and Swashbuckle to 11.0.1 in WebApi and WebApi.PostBuild. Updated the builder/runtime Docker image tags and the .NET target guidance in `AGENTS.md`.

### Why

The project targets, ASP.NET packages, OpenAPI packages, and container base images must align with .NET 11. The selected scope deliberately avoids moving to EF Core 11 because the current MySQL provider supports EF Core only through 10.

### What worked

The existing solution builds with the installed .NET 11 SDK when targeting .NET 10. Published package metadata confirmed that `Microsoft.AspNetCore.OpenApi` 11.0.0 requires `Microsoft.OpenApi` 3.11.0 or later, and that Swashbuckle 11.0.1 supports .NET 11 and OpenAPI.NET 3.11.

After retargeting and package updates, the first .NET 11 build exposed an OpenAPI.NET 3 interface type change in `OperationResponseFilterDescriptor`; changing the content dictionary to `IDictionary<string, IOpenApiMediaType>` fixed it. The subsequent full solution build succeeded with zero warnings and errors, and the WebApi post-build client generator completed.

The focused .NET 11 unit-test runs passed for WebApi.Tests (1,244 tests), Bff.WebApi.Tests (734 tests), Mvc.Tests (3,121 tests), and Repositories.Tests (334 tests). Both .NET 11 SDK and ASP.NET runtime Docker tags pulled successfully.

The Docker builder's restore and MVC/WebApi publish steps also succeeded on .NET 11. Its final BFF publish step could not run because the required local `sixlabors_license` build secret was not supplied.

### What didn't work

The baseline command `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"` did not complete in a reasonable time. Domain.Tests and BusinessLogic.Tests test hosts were still running after more than eight minutes, with no error message or completion result. The process was stopped, so this is an incomplete baseline rather than a reported test failure.

The first post-upgrade solution build failed in `OperationResponseFilterDescriptor.cs(67,36)` with `CS0266`: `Dictionary<string, Microsoft.OpenApi.OpenApiMediaType>` could not convert to `IDictionary<string, Microsoft.OpenApi.IOpenApiMediaType>`. The BFF and the other projects reached by the build compiled; this is a source compatibility change to fix in the WebApi OpenAPI filter.

The Docker command `docker build --target applicationbuilder -f OSDevGrp.OSIntranet.Builder.Dockerfile -t osintranet-builder-net11-check .` failed at the final publish step with `ERROR: secret sixlabors_license: not found`. The earlier restore and MVC/WebApi publish steps completed successfully.

Running Domain.Tests individually with `dotnet test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj --no-build --no-restore --filter "Category=UnitTest"` also produced no results after three minutes, so I stopped it. This reproduces the Domain test stall outside the solution-level run.

### What I learned

The WebApi build runs a post-build client-generation step, so generated client output must be checked after changing the OpenAPI dependency stack. The current EF Core/MySQL provider coupling needs to remain on EF Core 10 for this stage.

### What was tricky

The framework and OpenAPI package updates span many project files, while the EF-specific packages must remain on version 10. The filtered test command also stalled in two projects despite several other test projects passing.

### What warrants review

The updated solution builds cleanly, and focused WebApi, BFF WebApi, and MVC tests pass. The initial solution-wide filtered test run stalled in Domain.Tests and BusinessLogic.Tests, and the standalone Domain.Tests run stalled as well; those results remain incomplete. The BFF Docker publish and full container startup still require the local license secret and deployment configuration.

### Future work

Investigate the Domain/BusinessLogic test-host stalls, complete the BFF Docker publish through the established secure workflow, and revisit EF Core 11 when a compatible MySQL provider is available.
