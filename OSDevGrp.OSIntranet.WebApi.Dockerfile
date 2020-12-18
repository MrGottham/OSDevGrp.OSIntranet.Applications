FROM builder-osintranet AS applicationbuilder

FROM executor-osintranet AS applicationexecutor
WORKDIR /app
COPY --from=applicationbuilder /src/OSDevGrp.OSIntranet.WebApi/out .
COPY OSDevGrp.OSIntranet.WebApi.supervisord.conf /etc/supervisor/conf.d/supervisord.conf

ENV TZ=Europe/Copenhagen
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS http://*:80
ENV connectionStrings__OSIntranet=[TBD]
ENV Security__JWT__Key=[TBD]
ENV Security__AcmeChallenge__WellKnownChallengeToken=[TBD]
ENV Security__AcmeChallenge__ConstructedKeyAuthorization=[TBD]

EXPOSE 80 2222

ENTRYPOINT ["/usr/bin/supervisord"]