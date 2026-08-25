FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FastScan.Api/FastScan.Api.csproj", "FastScan.Api/"]
COPY ["FastScan.Models/FastScan.Models.csproj", "FastScan.Models/"]
COPY ["FastScan.Services/FastScan.Services.csproj", "FastScan.Services/"]
RUN dotnet restore "FastScan.Api/FastScan.Api.csproj"
COPY . .
RUN dotnet publish "FastScan.Api/FastScan.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "FastScan.Api.dll"]
