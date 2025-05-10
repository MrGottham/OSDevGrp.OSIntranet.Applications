FROM node:22.15-slim AS reactbase
ARG bffEndpointAddress
ENV SERVER_PORT=8080
ENV RUNNING_IN_CONTAINER=true
ENV VITE_BFF_ENDPOINT=${bffEndpointAddress}

FROM reactbase AS reactbuilder
WORKDIR /src
COPY osdevgrp.osintranet.react/package.json .
COPY osdevgrp.osintranet.react/package-lock.json .
RUN npm install

COPY osdevgrp.osintranet.react/. .
RUN npm run build

FROM reactbase AS reactexecutor
RUN apt-get update
RUN apt-get -y upgrade
RUN apt-get install -y supervisor openssh-server sudo

ARG appUserGroup
ARG nonRootUser
ARG nonRootPassword
ENV NON_ROOT_USER=${nonRootUser}
RUN groupadd ${appUserGroup}
RUN useradd -m -g ${appUserGroup} ${nonRootUser}
RUN usermod -a -G tty ${nonRootUser}
RUN usermod -a -G sudo ${nonRootUser}
RUN echo "${nonRootUser}:${nonRootPassword}" | chpasswd 

RUN mkdir -p /var/log/supervisor
RUN chmod g+rwx /var/run && chgrp ${appUserGroup} /var/run
RUN chmod g+rwx /var/log/supervisor && chgrp ${appUserGroup} /var/log/supervisor

ARG sshPassword
RUN mkdir /var/run/sshd
RUN echo "root:${sshPassword}" | chpasswd
RUN sed -i "s/#PermitRootLogin prohibit-password/PermitRootLogin yes/g" /etc/ssh/sshd_config
RUN sed -i "s/#Port 22/Port 2222/g" /etc/ssh/sshd_config

RUN echo "#!/bin/sh\nprintf \"${nonRootPassword}\"" > /usr/local/bin/askpw
RUN chmod 750 /usr/local/bin/askpw && chgrp ${appUserGroup} /usr/local/bin/askpw
ENV SUDO_ASKPASS=/usr/local/bin/askpw

RUN npm install -g serve

WORKDIR /app
COPY --from=reactbuilder /src/dist .
COPY OSDevGrp.OSIntranet.React.supervisord.conf /etc/supervisor/conf.d/supervisord.conf

ARG appUserGroup
ARG nonRootUser
RUN chmod g+rwx /app && chgrp ${appUserGroup} /app

EXPOSE 8080 2222

USER ${nonRootUser}

CMD ["/usr/bin/supervisord", "-n", "-c", "/etc/supervisor/conf.d/supervisord.conf"]