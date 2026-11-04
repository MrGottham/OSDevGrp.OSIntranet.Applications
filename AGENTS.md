# Repository notes for agents

## Verify changes

- .NET projects target `net10.0`. From the root: `dotnet build OSDevGrp.OSIntranet.Applications.sln`; run service-independent NUnit tests with `dotnet test OSDevGrp.OSIntranet.Applications.sln --filter "Category=UnitTest"` (unfiltered tests include integrations needing MySQL and external auth/services).
- For a focused test: `dotnet test OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj --filter "FullyQualifiedName~MethodName&Category=UnitTest"`; substitute the owning `*.Tests.csproj`. Test categories are on individual test methods. Reuse builders in `Core.TestHelpers`, `Domain.TestHelpers`, or `Bff.ServiceGateways.TestData` where applicable.
- Run frontend commands in `osdevgrp.osintranet.react/`: `npm ci`, `npm run lint`, `npm run build`, `npm run dev`. `package.json` has no frontend test or typecheck script. `vite.config.js` requires `VITE_BFF_ENDPOINT` for `dev` and proxies `/api` to it; outside containers it calls `dotnet dev-certs https` for HTTPS even during `build` (requires the .NET SDK).

## Boundaries and generated files

- `Core`/`Domain` hold shared and domain logic; `Repositories` uses EF Core with Oracle's MySQL provider; `BusinessLogic` is the application layer. Contracts live in the corresponding `*.Interfaces` projects, not the implementation projects.
- `WebApi` is the main REST API; `Mvc` is the server-rendered app. `Bff.WebApi` uses `Bff.DomainServices` and `Bff.ServiceGateways` to serve the React frontend; their interfaces are separate projects. React starts at `osdevgrp.osintranet.react/src/main.jsx`.
- `OSDevGrp.OSIntranet.WebApi/OSDevGrp.OSIntranet.WebApi.csproj` runs `postbuild` automatically after a build, generating `OSDevGrp.OSIntranet.WebApi.ClientApi/WebApiClient.generated.cs` via `WebApi.PostBuild` (Swashbuckle document -> NSwag client). Do not hand-edit the generated file. When changing API contracts, build `WebApi` before `WebApi.ClientApi`; the latter also builds a NuGet package.
- EF migrations target `Repositories` with the executable `Repositories.Migration` as startup project: `dotnet ef migrations add <Name> --project OSDevGrp.OSIntranet.Repositories --startup-project OSDevGrp.OSIntranet.Repositories.Migration` (same flags for `dotnet ef database update`). `RepositoryContext`'s parameterless design-time constructor reads .NET user secrets; its `OnConfiguring` requires `ConnectionStrings:OSIntranet`.

## Local configuration and conventions

- Root `.env`, frontend `.env`, `licenses/`, and `charts/osintranet/values.local.yaml` are gitignored local configuration; do not copy credentials into tracked files. `docker-compose.yaml` wires the full MVC/WebApi/BFF/React stack and requires those env values plus `licenses/sixlabors.lic` for the builder. `Bff.DomainServices.csproj` points `SixLaborsLicenseFile` at that license by default. AutoMapper mappings take `Licenses:AutoMapper:LicenseKey` from configuration.
- In development React uses the Vite `/api` proxy; production `src/main.jsx` loads `/runtime-config.json` before rendering, and `containerEntryPoint.mjs` generates it from `BFF_ENDPOINT_MAPPINGS`. Don't bake BFF endpoints into the frontend bundle.
- Existing C# code groups members in `#region` blocks and uses `NullGuard` and `IntranetExceptionBuilder` for guards/errors; match nearby files. Nullable and implicit usings vary by `.csproj` (do not assume solution-wide defaults).
