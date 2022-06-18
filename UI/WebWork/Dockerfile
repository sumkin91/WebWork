#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebWork/WebWork.csproj", "WebWork/"]
COPY ["WebWork.DAL/WebWork.DAL.csproj", "WebWork.DAL/"]
COPY ["WebWork.Domain/WebWork.Domain.csproj", "WebWork.Domain/"]
RUN dotnet restore "WebWork/WebWork.csproj"
COPY . .
WORKDIR "/src/WebWork"
RUN dotnet build "WebWork.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebWork.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebWork.dll"]