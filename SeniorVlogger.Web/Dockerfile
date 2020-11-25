#Build Backend
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS buildnet
WORKDIR /src
COPY ["SeniorVlogger.Web/SeniorVlogger.Web.csproj", "SeniorVlogger.Web/"]
COPY ["SeniorVlogger.DataAccess/SeniorVlogger.DataAccess.csproj", "SeniorVlogger.DataAccess/"]
COPY ["SeniorVlogger.Models/SeniorVlogger.Models.csproj", "SeniorVlogger.Models/"]
COPY ["SeniorVlogger.Common/SeniorVlogger.Common.csproj", "SeniorVlogger.Common/"]
RUN dotnet restore "SeniorVlogger.Web/SeniorVlogger.Web.csproj"
COPY . .
WORKDIR "/src/SeniorVlogger.Web"
RUN dotnet build "SeniorVlogger.Web.csproj" -c Release
RUN dotnet publish "SeniorVlogger.Web.csproj" -c Release -o /dist

#Build Frontend
FROM node:alpine as buildvue
WORKDIR /src
COPY SeniorVlogger.Web/ClientApp/package.json .
RUN npm install --silent 
COPY SeniorVlogger.Web/ClientApp .
RUN npm run --silent build

#Backend + Frontend production
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS https://+:9001
EXPOSE 9001

COPY --from=buildnet /dist .
COPY --from=buildvue /src/dist /app/ClientApp/dist
ENTRYPOINT ["dotnet", "SeniorVlogger.Web.dll"]