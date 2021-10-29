FROM builder-osintranet AS applicationbuilder

FROM executor-osintranet AS applicationexecutor
WORKDIR /app
COPY --from=applicationbuilder /src/OSDevGrp.OSIntranet.Mvc/out .
COPY OSDevGrp.OSIntranet.Mvc.supervisord.conf /etc/supervisor/conf.d/supervisord.conf

ENV connectionStrings__OSIntranet=[TBD]
ENV Security__Microsoft__ClientId=[TBD] Security__Microsoft__ClientSecret=[TBD] Security__Microsoft__Tenant=[TBD]
ENV Security__Google__ClientId=[TBD] Security__Google__ClientSecret=[TBD]
ENV Security__TrustedDomainCollection=[TBD]
ENV Security__AcmeChallenge__WellKnownChallengeToken=[TBD]
ENV Security__AcmeChallenge__ConstructedKeyAuthorization=[TBD]
ENV ExternalData__Dashboard__EndpointAddress=[TBD]

EXPOSE 80 2222

ENTRYPOINT ["/usr/bin/supervisord"]