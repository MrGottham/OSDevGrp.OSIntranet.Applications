FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
RUN apt-get update
RUN apt-get -y upgrade
RUN apt-get install -y supervisor openssh-server sudo
RUN apt-get install -y locales

RUN sed -i "s/^# *\(da_DK\)/\1/" /etc/locale.gen
RUN dpkg-reconfigure --frontend=noninteractive locales
RUN update-locale LANG=da_DK.UTF-8
ENV LANG=da_DK.UTF-8
ENV LANGUAGE=da_DK.da
ENV TZ=Europe/Copenhagen

ARG appUserGroup=[TBD]
ARG nonRootUser=[TBD]
ARG nonRootPassword=[TBD]
ENV NON_ROOT_USER=${nonRootUser}
RUN groupadd ${appUserGroup}
RUN useradd -m -g ${appUserGroup} ${nonRootUser}
RUN usermod -a -G tty ${nonRootUser}
RUN usermod -a -G sudo ${nonRootUser}
RUN echo "${nonRootUser}:${nonRootPassword}" | chpasswd 

RUN mkdir -p /var/log/supervisor
RUN chmod g+rwx /var/run && chgrp ${appUserGroup} /var/run
RUN chmod g+rwx /var/log/supervisor && chgrp ${appUserGroup} /var/log/supervisor

ARG sshPassword=[TBD]
RUN mkdir /var/run/sshd
RUN echo "root:${sshPassword}" | chpasswd
RUN sed -i "s/#PermitRootLogin prohibit-password/PermitRootLogin yes/g" /etc/ssh/sshd_config
RUN sed -i "s/#Port 22/Port 2222/g" /etc/ssh/sshd_config

RUN echo "#!/bin/sh\nprintf \"${nonRootPassword}\"" > /usr/local/bin/askpw
RUN chmod 750 /usr/local/bin/askpw && chgrp ${appUserGroup} /usr/local/bin/askpw
ENV SUDO_ASKPASS=/usr/local/bin/askpw

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_HTTP_PORTS=8080
