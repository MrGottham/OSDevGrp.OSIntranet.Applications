FROM builder-osintranet AS applicationbuilder

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=applicationbuilder /src/OSDevGrp.OSIntranet.WebApi/out .

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS http://*:80
ENV connectionStrings__OSDevGrp.OSIntranet=[TBD]
ENV Security__JWT__Key=[TBD]

EXPOSE 80

ENTRYPOINT ["dotnet", "OSDevGrp.OSIntranet.WebApi.dll"]