FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Services/Client/Client.API/Client.API.csproj Services/Client/Client.API/
COPY BlocCommun/BusEvenement/BusEvenement.csproj BlocCommun/BusEvenement/
COPY BlocCommun/RabbitMQBus/RabbitMQBus.csproj BlocCommun/RabbitMQBus/
RUN dotnet restore Services/Client/Client.API/Client.API.csproj
COPY . .
WORKDIR /src/Services/Client/Client.API
RUN dotnet build Client.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Client.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Client.API.dll"]
