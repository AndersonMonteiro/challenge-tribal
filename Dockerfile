FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Fintech.CreditLine.Api/Fintech.CreditLine.Api.csproj", "Fintech.CreditLine.Api/"]
COPY ["Fintech.CreditLine.Services/Fintech.CreditLine.Services.csproj", "Fintech.CreditLine.Services/"]
COPY ["Fintech.CreditLine.Domain/Fintech.CreditLine.Domain.csproj", "Fintech.CreditLine.Domain/"]
COPY ["Fintech.CreditLine.Data/Fintech.CreditLine.Data.csproj", "Fintech.CreditLine.Data/"]
RUN dotnet restore "Fintech.CreditLine.Api/Fintech.CreditLine.Api.csproj"
COPY . .
WORKDIR "/src/Fintech.CreditLine.Api"
RUN dotnet build "Fintech.CreditLine.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fintech.CreditLine.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Development"
ENTRYPOINT ["dotnet", "Fintech.CreditLine.Api.dll", "--urls", "http://*:5000;http://*:5001"]