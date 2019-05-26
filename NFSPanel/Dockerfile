FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base

RUN apt-get update
RUN apt-get install -y nfs-kernel-server nfs-common

ENV ACCOUNT=admin
ENV PASSWORD=admin

ENV Exports=/etc/exports
ENV NFS=/etc/init.d/nfs-kernel-server

WORKDIR /app
EXPOSE 80 443
EXPOSE 111/udp 111/tcp 2049/udp 2049/tcp

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-stretch AS build
WORKDIR /src
COPY ["NFSPanel/NFSPanel.csproj", "NFSPanel/"]
RUN dotnet restore "NFSPanel/NFSPanel.csproj"
COPY . .
WORKDIR "/src/NFSPanel"
RUN dotnet build "NFSPanel.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NFSPanel.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
USER root

RUN echo "service rpcbind restart\n" >> start.sh
RUN echo "dotnet NFSPanel.dll" >> start.sh

ENTRYPOINT ["bash", "start.sh"]
