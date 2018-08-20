# Build .NET code
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app
COPY ./ ./build
WORKDIR /app/build/PersonalWebsite
RUN dotnet publish -c Release -o ../out

# Build client code
FROM node:8.11.3 as clientBuild
WORKDIR /app
COPY PersonalWebsite/ ./build
WORKDIR /app/build/PersonalWebsite
RUN npm install --production
RUN npm run-script build

# Setup runtime
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app

# copy build
COPY --from=build /app/build/out ./
COPY --from=clientBuild /app/build/wwwroot/js/Build ./wwwroot/js/Build
COPY --from=clientBuild /app/build/wwwroot/css/Build ./wwwroot/css/Build

# install nginx
ENV NGINX_VERSION 1.15.0-1~stretch
ENV NJS_VERSION   1.15.0.0.2.1-1~stretch

RUN apt-get update \
	&& apt-get install --no-install-recommends --no-install-suggests -y gnupg1 apt-transport-https ca-certificates \
	&& \
	NGINX_GPGKEY=573BFD6B3D8FBC641079A6ABABF5BD827BD9BF62; \
	found=''; \
	for server in \
		ha.pool.sks-keyservers.net \
		hkp://keyserver.ubuntu.com:80 \
		hkp://p80.pool.sks-keyservers.net:80 \
		pgp.mit.edu \
	; do \
		echo "Fetching GPG key $NGINX_GPGKEY from $server"; \
		apt-key adv --keyserver "$server" --keyserver-options timeout=10 --recv-keys "$NGINX_GPGKEY" && found=yes && break; \
	done; \
	test -z "$found" && echo >&2 "error: failed to fetch GPG key $NGINX_GPGKEY" && exit 1; \
	apt-get remove --purge --auto-remove -y gnupg1 && rm -rf /var/lib/apt/lists/* \
	&& dpkgArch="$(dpkg --print-architecture)" \
	&& nginxPackages=" \
		nginx=${NGINX_VERSION} \
		nginx-module-xslt=${NGINX_VERSION} \
		nginx-module-geoip=${NGINX_VERSION} \
		nginx-module-image-filter=${NGINX_VERSION} \
		nginx-module-njs=${NJS_VERSION} \
	" \
	&& echo "deb https://nginx.org/packages/mainline/debian/ stretch nginx" >> /etc/apt/sources.list.d/nginx.list \
	&& apt-get update \
	&& apt-get install --no-install-recommends --no-install-suggests -y \
						$nginxPackages \
						gettext-base

# forward nginx log to docker out/err
RUN ln -sf /dev/stdout /var/log/nginx/access.log \
	&& ln -sf /dev/stderr /var/log/nginx/error.log

RUN openssl dhparam -out /etc/ssl/certs/dhparam.pem 2048

EXPOSE 80

ENV ASPNETCORE_URLS=http://+:5000

RUN rm /etc/nginx/conf.d/default.conf

ENTRYPOINT service nginx start && dotnet "PersonalWebsite.dll"
