FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Movies.Api/Movies.Api.csproj", "Movies.Api/"]
COPY ["Movies.Application/Movies.Application.csproj", "Movies.Application/"]
COPY ["Movies.Contracts/Movies.Contracts.csproj", "Movies.Contracts/"]
RUN dotnet restore "Movies.Api/Movies.Api.csproj"

COPY . .
WORKDIR "/src/Movies.Api"
RUN dotnet build "Movies.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movies.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.Api.dll"]

