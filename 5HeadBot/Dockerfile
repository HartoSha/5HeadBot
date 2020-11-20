FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

# https://github.com/docker/buildx/issues/132
#####################
# PUPPETEER  RECIPE 
#####################
# Install latest chrome dev package and fonts to support major charsets (Chinese, Japanese, Arabic, Hebrew, Thai and a few others)
# Note: this installs the necessary libs to make the bundled version of Chromium that Puppeteer
# installs, work.
WORKDIR /usr/bin
ARG CHROME_VERSION="81.0.4044.138-1"
RUN apt-get update \
	&& apt-get -f install \
	&& apt-get -y install wget gnupg2 apt-utils
RUN wget --no-verbose -O /tmp/chrome.deb http://dl.google.com/linux/chrome/deb/pool/main/g/google-chrome-stable/google-chrome-stable_${CHROME_VERSION}_amd64.deb \
	&& apt-get update \
	&& apt-get install -y /tmp/chrome.deb --no-install-recommends --allow-downgrades \
	&& rm /tmp/chrome.deb
ENV PUPPETEER_EXECUTABLE_PATH "/usr/bin/google-chrome"
#####################
#END PUPPETEER RECIPE
#####################

WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["/5HeadBot.csproj", "5HeadBot/"]
RUN dotnet restore "5HeadBot/5HeadBot.csproj"
COPY . .
RUN dotnet build "5HeadBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "5HeadBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app/publish
COPY --from=publish /app/publish .

EXPOSE 80
CMD ["dotnet", "5HeadBot.dll"]