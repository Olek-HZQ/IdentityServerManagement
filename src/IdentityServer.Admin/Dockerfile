#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/IdentityServer.Admin/IdentityServer.Admin.csproj", "src/IdentityServer.Admin/"]
COPY ["src/IdentityServer.Admin.Services/IdentityServer.Admin.Services.csproj", "src/IdentityServer.Admin.Services/"]
COPY ["src/IdentityServer.Admin.Dapper/IdentityServer.Admin.Dapper.csproj", "src/IdentityServer.Admin.Dapper/"]
COPY ["src/IdentityServer.Admin.Core/IdentityServer.Admin.Core.csproj", "src/IdentityServer.Admin.Core/"]
RUN dotnet restore "src/IdentityServer.Admin/IdentityServer.Admin.csproj"
COPY . .
WORKDIR "/src/src/IdentityServer.Admin"
RUN dotnet build "IdentityServer.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.Admin.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Production
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN apt-get update -y
RUN apt-get install vim -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.Admin.dll"]