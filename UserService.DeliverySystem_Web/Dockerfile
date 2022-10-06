#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserService.DeliverySystem_Web/UserService.DeliverySystem_Web.csproj", "UserService.DeliverySystem_Web/"]
COPY ["DeliverySystem_MappingProfile/DeliverySystem_MappingProfile.csproj", "DeliverySystem_MappingProfile/"]
COPY ["RestaurantService.DeliverySystem_DAL/RestaurantService.DeliverySystem_DAL.csproj", "RestaurantService.DeliverySystem_DAL/"]
COPY ["UserService.DeliverySystem_DAL/UserService.DeliverySystem_DAL.csproj", "UserService.DeliverySystem_DAL/"]
COPY ["DeliverySystem_Common/DeliverySystem_Common.csproj", "DeliverySystem_Common/"]
COPY ["UserService.DeliverySystem_Service/UserService.DeliverySystem_BAL.csproj", "UserService.DeliverySystem_Service/"]
RUN dotnet restore "UserService.DeliverySystem_Web/UserService.DeliverySystem_Web.csproj"
COPY . .
WORKDIR "/src/UserService.DeliverySystem_Web"
RUN dotnet build "UserService.DeliverySystem_Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.DeliverySystem_Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.DeliverySystem_Web.dll"]