FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 52464
EXPOSE 44331

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Services/Magasin/Magasin.API/Magasin.API.csproj", "Services/Magasin/Magasin.API/"]
COPY ["BlocCommun/BusEvenement/BusEvenement.csproj", "BlocCommun/BusEvenement/"]
COPY ["BlocCommun/RabbitMQBus/RabbitMQBus.csproj", "BlocCommun/RabbitMQBus/"]
RUN dotnet restore "Services/Magasin/Magasin.API/Magasin.API.csproj"
COPY . .
WORKDIR "/src/Services/Magasin/Magasin.API"
RUN dotnet build "Magasin.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Magasin.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Magasin.API.dll"]