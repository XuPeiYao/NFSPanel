FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base

ENV ACCOUNT=admin
ENV PASSWORD=admin

ENV Exports=/etc/exports
ENV NFS=/etc/init.d/nfs-kernel-server

WORKDIR /app
EXPOSE 80
EXPOSE 443

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
ENTRYPOINT ["dotnet", "NFSPanel.dll"]