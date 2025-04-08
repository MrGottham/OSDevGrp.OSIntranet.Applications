FROM emberstack/openssl AS certificatebuilder
WORKDIR /certificate

COPY OSDevGrp.OSIntranet.CertificateBuilder.Certificate.conf certificate.conf

ARG certificateCommonName
ARG certificateCountryName
ARG certificateStateOrProvinceName
ARG certificateLocalityName
ARG certificateOrganizationName
ARG certificateOrganizationalUnitName
ARG certificateDns1
ARG certificateDns2
ARG certificateDns3
ARG certificateDns4
ARG certificateDns5
ARG certificatePassword

RUN sed -i "s/\[certificateDns1\]/${certificateDns1}/g" certificate.conf
RUN sed -i "s/\[certificateDns2\]/${certificateDns2}/g" certificate.conf
RUN sed -i "s/\[certificateDns3\]/${certificateDns3}/g" certificate.conf
RUN sed -i "s/\[certificateDns4\]/${certificateDns4}/g" certificate.conf
RUN sed -i "s/\[certificateDns5\]/${certificateDns5}/g" certificate.conf

RUN openssl req -x509 -nodes -days 365 -newkey rsa:4096 -keyout localhost.key -out localhost.crt -config certificate.conf -subj "/C=${certificateCountryName}/ST=${certificateStateOrProvinceName}/L=${certificateLocalityName}/O=${certificateOrganizationName}/OU${certificateOrganizationalUnitName}=/CN=localhost" -passin pass:${certificatePassword}
RUN openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt -password pass:${certificatePassword}

RUN openssl req -x509 -nodes -days 365 -newkey rsa:4096 -keyout ${certificateCommonName}.key -out ${certificateCommonName}.crt -config certificate.conf -subj "/C=${certificateCountryName}/ST=${certificateStateOrProvinceName}/L=${certificateLocalityName}/O=${certificateOrganizationName}/OU${certificateOrganizationalUnitName}=/CN=${certificateCommonName}" -passin pass:${certificatePassword}
RUN openssl pkcs12 -export -out ${certificateCommonName}.pfx -inkey ${certificateCommonName}.key -in ${certificateCommonName}.crt -password pass:${certificatePassword}