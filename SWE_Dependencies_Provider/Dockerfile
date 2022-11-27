FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Common/Common.csproj", "Common/"]
COPY ["DataAccessLayer/DataAccessLayer.csproj", "DataAccessLayer/"]
COPY ["BusinessLogicLayer/BusinessLogicLayer.csproj", "BusinessLogicLayer/"]
COPY ["SWE_Dependencies_Provider/SWE_Dependencies_Provider.csproj", "SWE_Dependencies_Provider/"]
RUN dotnet restore "SWE_Dependencies_Provider/SWE_Dependencies_Provider.csproj"
COPY . .
WORKDIR "/src/SWE_Dependencies_Provider"
RUN dotnet build "SWE_Dependencies_Provider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SWE_Dependencies_Provider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SWE_Dependencies_Provider.dll"]
