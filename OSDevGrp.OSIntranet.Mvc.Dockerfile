FROM builder-osintranet AS applicationbuilder

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=applicationbuilder /src/OSDevGrp.OSIntranet.Mvc/out .

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS http://*:80
ENV connectionStrings__OSDevGrp.OSIntranet=[TBD]
ENV Security__Microsoft__ClientId=[TBD] Security__Microsoft__ClientSecret=[TBD] Security__Microsoft__Tenant=[TBD]
ENV Security__Google__ClientId=[TBD] Security__Google__ClientSecret=[TBD]
ENV Security__TrustedDomainCollection=[TBD]

EXPOSE 80

ENTRYPOINT ["dotnet", "OSDevGrp.OSIntranet.Mvc.dll"]