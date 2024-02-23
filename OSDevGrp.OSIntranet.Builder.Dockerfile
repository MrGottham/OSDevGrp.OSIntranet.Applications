FROM mcr.microsoft.com/dotnet/sdk:8.0 AS applicationbuilder
WORKDIR /src

# Copy .sln and .csproj files and restore as distinct layers
COPY OSDevGrp.OSIntranet.Applications.sln .
COPY OSDevGrp.OSIntranet.Core.Interfaces/OSDevGrp.OSIntranet.Core.Interfaces.csproj ./OSDevGrp.OSIntranet.Core.Interfaces/
COPY OSDevGrp.OSIntranet.Core/OSDevGrp.OSIntranet.Core.csproj ./OSDevGrp.OSIntranet.Core/
COPY OSDevGrp.OSIntranet.Core.TestHelpers/OSDevGrp.OSIntranet.Core.TestHelpers.csproj ./OSDevGrp.OSIntranet.Core.TestHelpers/
COPY OSDevGrp.OSIntranet.Core.Tests/OSDevGrp.OSIntranet.Core.Tests.csproj ./OSDevGrp.OSIntranet.Core.Tests/
COPY OSDevGrp.OSIntranet.Domain.Interfaces/OSDevGrp.OSIntranet.Domain.Interfaces.csproj ./OSDevGrp.OSIntranet.Domain.Interfaces/
COPY OSDevGrp.OSIntranet.Domain/OSDevGrp.OSIntranet.Domain.csproj ./OSDevGrp.OSIntranet.Domain/
COPY OSDevGrp.OSIntranet.Domain.TestHelpers/OSDevGrp.OSIntranet.Domain.TestHelpers.csproj ./OSDevGrp.OSIntranet.Domain.TestHelpers/
COPY OSDevGrp.OSIntranet.Domain.Tests/OSDevGrp.OSIntranet.Domain.Tests.csproj ./OSDevGrp.OSIntranet.Domain.Tests/
COPY OSDevGrp.OSIntranet.Repositories.Interfaces/OSDevGrp.OSIntranet.Repositories.Interfaces.csproj ./OSDevGrp.OSIntranet.Repositories.Interfaces/
COPY OSDevGrp.OSIntranet.Repositories/OSDevGrp.OSIntranet.Repositories.csproj ./OSDevGrp.OSIntranet.Repositories/
COPY OSDevGrp.OSIntranet.Repositories.Migration/OSDevGrp.OSIntranet.Repositories.Migration.csproj ./OSDevGrp.OSIntranet.Repositories.Migration/
COPY OSDevGrp.OSIntranet.Repositories.Tests/OSDevGrp.OSIntranet.Repositories.Tests.csproj ./OSDevGrp.OSIntranet.Repositories.Tests/
COPY OSDevGrp.OSIntranet.BusinessLogic.Interfaces/OSDevGrp.OSIntranet.BusinessLogic.Interfaces.csproj ./OSDevGrp.OSIntranet.BusinessLogic.Interfaces/
COPY OSDevGrp.OSIntranet.BusinessLogic/OSDevGrp.OSIntranet.BusinessLogic.csproj ./OSDevGrp.OSIntranet.BusinessLogic/
COPY OSDevGrp.OSIntranet.BusinessLogic.Tests/OSDevGrp.OSIntranet.BusinessLogic.Tests.csproj ./OSDevGrp.OSIntranet.BusinessLogic.Tests/
COPY OSDevGrp.OSIntranet.Mvc/OSDevGrp.OSIntranet.Mvc.csproj ./OSDevGrp.OSIntranet.Mvc/
COPY OSDevGrp.OSIntranet.Mvc.Tests/OSDevGrp.OSIntranet.Mvc.Tests.csproj ./OSDevGrp.OSIntranet.Mvc.Tests/
COPY OSDevGrp.OSIntranet.WebApi/OSDevGrp.OSIntranet.WebApi.csproj ./OSDevGrp.OSIntranet.WebApi/
COPY OSDevGrp.OSIntranet.WebApi.Tests/OSDevGrp.OSIntranet.WebApi.Tests.csproj ./OSDevGrp.OSIntranet.WebApi.Tests/
RUN dotnet restore

# Copy everything else and build app
COPY . .

# Build the MVC application
WORKDIR /src/OSDevGrp.OSIntranet.Mvc
RUN dotnet publish -c Release -o out

# Build the WebApi application
WORKDIR /src/OSDevGrp.OSIntranet.WebApi
RUN dotnet publish -c Release -o out