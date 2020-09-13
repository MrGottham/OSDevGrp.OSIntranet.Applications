FROM debian
RUN apt-get update
RUN apt-get -y upgrade
RUN apt-get install -y openssh-server certbot openssl

ARG sshPassword=[TBD]
RUN mkdir /var/run/sshd
RUN echo "root:${sshPassword}" | chpasswd
RUN sed -i "s/#PermitRootLogin prohibit-password/PermitRootLogin yes/g" /etc/ssh/sshd_config
RUN sed -i "s/#Port 22/Port 2222/g" /etc/ssh/sshd_config

EXPOSE 2222
VOLUME ["/etc/letsencrypt", "/var/lib/letsencrypt"]

CMD ["/usr/sbin/sshd", "-D"]