FROM certificatebuilder-osintranet AS certificatebuilder

FROM nginx:latest AS webserver
COPY Nginx/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=certificatebuilder /certificate/localhost.crt /etc/ssl/certs/localhost.crt
COPY --from=certificatebuilder /certificate/localhost.key /etc/ssl/private/localhost.key

ARG certificateCommonName
COPY --from=certificatebuilder /certificate/${certificateCommonName}.crt /etc/ssl/certs/${certificateCommonName}.crt
COPY --from=certificatebuilder /certificate/${certificateCommonName}.key /etc/ssl/private/${certificateCommonName}.key

EXPOSE 80 443 8443 8445 8447