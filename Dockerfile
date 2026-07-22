FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY src/ErpInventory.Domain/*.csproj src/ErpInventory.Domain/
COPY src/ErpInventory.Application/*.csproj src/ErpInventory.Application/
COPY src/ErpInventory.Infrastructure/*.csproj src/ErpInventory.Infrastructure/
COPY src/ErpInventory.Api/*.csproj src/ErpInventory.Api/
RUN dotnet restore src/ErpInventory.Api/ErpInventory.Api.csproj

COPY src/ src/
RUN dotnet publish src/ErpInventory.Api/ErpInventory.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ErpInventory.Api.dll"]