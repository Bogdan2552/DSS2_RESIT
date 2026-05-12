FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MovieWatchlist.Api.csproj", "./"]
RUN dotnet restore "MovieWatchlist.Api.csproj"
COPY . .
RUN dotnet publish "MovieWatchlist.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN mkdir -p /app/data
EXPOSE 4058
ENV ASPNETCORE_URLS=http://+:4058
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MovieWatchlist.Api.dll"]
