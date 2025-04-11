FROM builder-osintranet AS applicationbuilder

FROM executor-osintranet AS applicationexecutor
WORKDIR /app
COPY --from=applicationbuilder /src/OSDevGrp.OSIntranet.WebApi/out .
COPY OSDevGrp.OSIntranet.WebApi.supervisord.conf /etc/supervisor/conf.d/supervisord.conf

ARG appUserGroup
ARG nonRootUser
RUN chmod g+rwx /app && chgrp ${appUserGroup} /app

EXPOSE 8080 2222

USER ${nonRootUser}

ENTRYPOINT ["/usr/bin/supervisord", "-n", "-c", "/etc/supervisor/conf.d/supervisord.conf"]