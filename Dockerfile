FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80/tcp

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CodeFood_API.Asnan.csproj", "."]
RUN dotnet restore "./CodeFood_API.Asnan.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CodeFood_API.Asnan.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeFood_API.Asnan.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeFood_API.Asnan.dll"]