FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
WORKDIR /app

COPY . .

RUN dotnet restore "src/KittysolomaMap.Api/KittysolomaMap.Api.csproj"
RUN dotnet publish "src/KittysolomaMap.Api/KittysolomaMap.Api.csproj" -c Release -o /dist 

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runner
WORKDIR /app

EXPOSE 80
ENV ASPNETCORE_URLS: "http://+:80"

COPY --from=builder /dist /app

ENTRYPOINT ["dotnet", "KittysolomaMap.Api.dll"]