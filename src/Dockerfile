FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Adapters/Driver/FoodTotem.Payment.API/FoodTotem.Payment.API.csproj", "Adapters/Driver/FoodTotem.Payment.API/"]
COPY ["Core/FoodTotem.Payment.UseCase/FoodTotem.Payment.UseCase.csproj", "Core/FoodTotem.Payment.UseCase/"]
COPY ["Core/FoodTotem.Payment.Domain/FoodTotem.Payment.Domain.csproj", "Core/FoodTotem.Payment.Domain/"]
COPY ["Core/FoodTotem.Domain.Core/FoodTotem.Domain.Core.csproj", "Core/FoodTotem.Domain.Core/"]
COPY ["Core/FoodTotem.Data.Core/FoodTotem.Data.Core.csproj", "Core/FoodTotem.Data.Core/"]
COPY ["Adapters/Driven/FoodTotem.Payment.Gateways.MongoDB/FoodTotem.Payment.Gateways.MongoDB.csproj", "Adapters/Driven/FoodTotem.Payment.Gateways.MongoDB/"]
COPY ["Adapters/Driven/FoodTotem.Payment.Gateways.Http/FoodTotem.Payment.Gateways.Http.csproj", "Adapters/Driven/FoodTotem.Payment.Gateways.Http/"]
COPY ["Adapters/Driven/FoodTotem.Payment.Gateways.MercadoPago/FoodTotem.Payment.Gateways.MercadoPago.csproj", "Adapters/Driven/FoodTotem.Payment.Gateways.MercadoPago/"]

RUN dotnet restore "Adapters/Driver/FoodTotem.Payment.API/FoodTotem.Payment.API.csproj"
COPY . .
WORKDIR "/src/Adapters/Driver/FoodTotem.Payment.API"
RUN dotnet build "FoodTotem.Payment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodTotem.Payment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodTotem.Payment.API.dll"]