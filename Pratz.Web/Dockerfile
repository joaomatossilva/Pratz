FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Pratz.Web/Pratz.Web.csproj", "Pratz.Web/"]
RUN dotnet restore "Pratz.Web/Pratz.Web.csproj"
COPY . .
WORKDIR "/src/Pratz.Web"
RUN dotnet build "Pratz.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pratz.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pratz.Web.dll"]