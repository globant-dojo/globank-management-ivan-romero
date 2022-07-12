#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 446

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReportesApp/ReportesApp.csproj", "ReportesApp/"]
COPY ["GenericRepository/GenericRepository.csproj", "GenericRepository/"]
RUN dotnet restore "ReportesApp/ReportesApp.csproj"
COPY . .
WORKDIR "/src/ReportesApp"
RUN dotnet build "ReportesApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReportesApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportesApp.dll"]