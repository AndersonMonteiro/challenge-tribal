FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Tribal.CreditLine.Api/Tribal.CreditLine.Api.csproj", "Tribal.CreditLine.Api/"]
COPY ["Tribal.CreditLine.Services/Tribal.CreditLine.Services.csproj", "Tribal.CreditLine.Services/"]
COPY ["Tribal.CreditLine.Domain/Tribal.CreditLine.Domain.csproj", "Tribal.CreditLine.Domain/"]
COPY ["Tribal.CreditLine.Data/Tribal.CreditLine.Data.csproj", "Tribal.CreditLine.Data/"]
RUN dotnet restore "Tribal.CreditLine.Api/Tribal.CreditLine.Api.csproj"
COPY . .
WORKDIR "/src/Tribal.CreditLine.Api"
RUN dotnet build "Tribal.CreditLine.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tribal.CreditLine.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Development"
ENTRYPOINT ["dotnet", "Tribal.CreditLine.Api.dll", "--urls", "http://*:5000;http://*:5001"]