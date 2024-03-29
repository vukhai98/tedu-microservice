FROM mcr.microsoft.com/dotnet/aspnet:6.0 As base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build 
WORKDIR /src

COPY ["Services/Ordering/Ordering.API/Ordering.API.csproj","Services/Ordering/Ordering.API/"] 
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj","BuildingBlocks/Common.Logging/"] 
COPY ["BuildingBlocks/Contracts/Contracts.csproj","BuildingBlocks/Contracts/"] 
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj","BuildingBlocks/Infrastructure/"] 
COPY ["Services/Shared/Shared.csproj","BuildingBlocks/Shared/"] 
COPY ["Services/Ordering/Ordering.Application/Ordering.Application.csproj","Services/Ordering/Ordering.Application/"] 
COPY ["Services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj","Services/Ordering/Ordering.Infrastructure/"] 
COPY ["Services/Ordering/Ordering.Domain/Ordering.Domain.csproj","Services/Ordering/Ordering.Domain/"] 
COPY ["BuildingBlocks/EventBus/EventBus.Messages/EventBus.Messages/EventBus.Messages.csproj","BuildingBlocks/EventBus/EventBus.Messages/EventBus.Messages/"] 


RUN  dotnet restore "Services/Ordering/Ordering.API/Ordering.API.csproj"

COPY . .
WORKDIR  "/src/Services/Ordering/Ordering.API"
RUN  dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN  dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base as Final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet","Ordering.API.dll"]