FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Services/SimulationFlux/SimulationFlux.API/SimulationFlux.API.csproj", "Services/SimulationFlux/SimulationFlux.API/"]
RUN dotnet restore "Services/SimulationFlux/SimulationFlux.API/SimulationFlux.API.csproj"
COPY . .
WORKDIR "/src/Services/SimulationFlux/SimulationFlux.API"
RUN dotnet build "SimulationFlux.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SimulationFlux.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SimulationFlux.API.dll"]