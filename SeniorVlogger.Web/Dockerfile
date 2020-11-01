#Build Backend
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine as buildnet
WORKDIR /src
COPY ["SeniorVlogger.Web/SeniorVlogger.Web.csproj", "SeniorVlogger.Web/"]
COPY ["SeniorVlogger.DataAccess/SeniorVlogger.DataAccess.csproj", "SeniorVlogger.DataAccess/"]
COPY ["SeniorVlogger.Models/SeniorVlogger.Models.csproj", "SeniorVlogger.Models/"]
COPY ["SeniorVlogger.Common/SeniorVlogger.Common.csproj", "SeniorVlogger.Common/"]
RUN dotnet restore "SeniorVlogger.Web/SeniorVlogger.Web.csproj"
COPY . .
WORKDIR "/src/SeniorVlogger.Web"
RUN dotnet build "SeniorVlogger.Web.csproj" --no-incremental -c Release
RUN dotnet publish "SeniorVlogger.Web.csproj" -c Release -o /dist

#Build Frontend
FROM node:alpine as buildvue
WORKDIR /src
COPY SeniorVlogger.Web/ClientApp/package.json .
RUN npm install --silent
COPY SeniorVlogger.Web/ClientApp .
RUN npm run --silent build

#Backend + Frontend production
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS http://+:9001
EXPOSE 9001

COPY --from=buildnet /dist .
COPY --from=buildvue /src/dist /app/ClientApp/dist
ENTRYPOINT ["dotnet", "SeniorVlogger.Web.dll"]