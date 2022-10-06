#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RestaurantService.DeliverySystem_Web/RestaurantService.DeliverySystem_Web.csproj", "RestaurantService.DeliverySystem_Web/"]
COPY ["DeliverySystem_MappingProfile/DeliverySystem_MappingProfile.csproj", "DeliverySystem_MappingProfile/"]
COPY ["RestaurantService.DeliverySystem_DAL/RestaurantService.DeliverySystem_DAL.csproj", "RestaurantService.DeliverySystem_DAL/"]
COPY ["UserService.DeliverySystem_DAL/UserService.DeliverySystem_DAL.csproj", "UserService.DeliverySystem_DAL/"]
COPY ["DeliverySystem_Common/DeliverySystem_Common.csproj", "DeliverySystem_Common/"]
COPY ["RestaurantSercice.DeliverySystem_BAL/RestaurantService.DeliverySystem_BAL.csproj", "RestaurantSercice.DeliverySystem_BAL/"]
RUN dotnet restore "RestaurantService.DeliverySystem_Web/RestaurantService.DeliverySystem_Web.csproj"
COPY . .
WORKDIR "/src/RestaurantService.DeliverySystem_Web"
RUN dotnet build "RestaurantService.DeliverySystem_Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantService.DeliverySystem_Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantService.DeliverySystem_Web.dll"]