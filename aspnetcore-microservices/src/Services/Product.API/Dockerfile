FROM mcr.microsoft.com/dotnet/aspnet:6.0 As base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build 
WORKDIR /src
COPY ["Services/Product.API/Product.API.csproj","Services/Product.API/"] 
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj","BuildingBlocks/Common.Logging/"] 
COPY ["BuildingBlocks/Contracts/Contracts.csproj","BuildingBlocks/Contracts/"] 
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj","BuildingBlocks/Infrastructure/"] 
COPY ["Services/Shared/Shared.csproj","BuildingBlocks/Shared/"] 

RUN  dotnet restore "Services/Product.API/Product.API.csproj"

COPY . .
WORKDIR  "/src/Services/Product.API"
RUN  dotnet build "Product.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN  dotnet publish "Product.API.csproj" -c Release -o /app/publish

FROM base as Final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet","Product.API.dll"]