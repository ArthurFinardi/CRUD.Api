FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CRUD.Api/CRUD.Api.csproj", "CRUD.Api/"]
COPY ["CRUD.Application/CRUD.Application.csproj", "CRUD.Application/"]
COPY ["CRUD.Domain/CRUD.Domain.csproj", "CRUD.Domain/"]
COPY ["CRUD.Infrastructure/CRUD.Infrastructure.csproj", "CRUD.Infrastructure/"]
RUN dotnet restore "CRUD.Api/CRUD.Api.csproj"
COPY . .
WORKDIR "/src/CRUD.Api"
RUN dotnet build "CRUD.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRUD.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRUD.Api.dll"] 