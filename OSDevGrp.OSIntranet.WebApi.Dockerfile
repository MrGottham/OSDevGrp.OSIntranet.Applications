FROM builder-osintranet AS applicationbuilder

FROM executor-osintranet AS applicationexecutor
WORKDIR /app
COPY --from=applicationbuilder /src/OSDevGrp.OSIntranet.WebApi/out .
COPY OSDevGrp.OSIntranet.WebApi.supervisord.conf /etc/supervisor/conf.d/supervisord.conf

ARG appUserGroup=[TBD]
ARG nonRootUser=[TBD]
RUN chmod g+rwx /app && chgrp ${appUserGroup} /app

ENV connectionStrings__OSIntranet=[TBD]
ENV Security__Microsoft__ClientId=[TBD] Security__Microsoft__ClientSecret=[TBD] Security__Microsoft__Tenant=[TBD]
ENV Security__Google__ClientId=[TBD] Security__Google__ClientSecret=[TBD]
ENV Security__TrustedDomainCollection=[TBD]
ENV Security__JWT__Key__kty=[TBD]
ENV Security__JWT__Key__n=[TBD]
ENV Security__JWT__Key__e=[TBD]
ENV Security__JWT__Key__d=[TBD]
ENV Security__JWT__Key__dp=[TBD]
ENV Security__JWT__Key__dq=[TBD]
ENV Security__JWT__Key__p=[TBD]
ENV Security__JWT__Key__q=[TBD]
ENV Security__JWT__Key__qi=[TBD]
ENV Security__JWT__Issuer=[TBD]
ENV Security__JWT__Audience=[TBD]
ENV Security__AcmeChallenge__WellKnownChallengeToken=[TBD]
ENV Security__AcmeChallenge__ConstructedKeyAuthorization=[TBD]

EXPOSE 8080 2222

USER ${nonRootUser}

ENTRYPOINT ["/usr/bin/supervisord", "-n", "-c", "/etc/supervisor/conf.d/supervisord.conf"]