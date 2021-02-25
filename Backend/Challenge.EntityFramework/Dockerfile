FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

LABEL author="Helton Reis"

ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
	&& tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
	&& rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

RUN chown -R www-data:www-data /var/www
RUN chown -R www-data:www-data /app

#RUN usermod -u 1000 www-data

#USER www-data

RUN usermod -u 1000 root

USER root

RUN dotnet tool install --global dotnet-ef
ENV PATH /var/www/.dotnet/tools:$PATH

ENTRYPOINT ["./entrypoint.sh"]