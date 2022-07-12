#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 444

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CuentasApp/CuentasApp.csproj", "CuentasApp/"]
COPY ["GenericRepository/GenericRepository.csproj", "GenericRepository/"]
RUN dotnet restore "CuentasApp/CuentasApp.csproj"
COPY . .
WORKDIR "/src/CuentasApp"
RUN dotnet build "CuentasApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CuentasApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CuentasApp.dll"]