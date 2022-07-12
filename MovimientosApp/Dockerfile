#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 445

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MovimientosApp/MovimientosApp.csproj", "MovimientosApp/"]
COPY ["GenericRepository/GenericRepository.csproj", "GenericRepository/"]
RUN dotnet restore "MovimientosApp/MovimientosApp.csproj"
COPY . .
WORKDIR "/src/MovimientosApp"
RUN dotnet build "MovimientosApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MovimientosApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovimientosApp.dll"]