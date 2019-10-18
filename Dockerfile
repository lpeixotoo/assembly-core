FROM ubuntu:18.04

ARG DOTNET_VERSION=3.0

RUN apt-get update \
    && apt-get install -y \
       gcc \
       make \
       wget \
       git  \
       sudo \
       gnupg

RUN wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update -qq \
    && apt-get install -y dotnet-sdk-"$DOTNET_VERSION"

WORKDIR /home/app

COPY ./ ./

ENTRYPOINT dotnet test
