FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SWE_Consumer2/SWE_Consumer2.csproj", "SWE_Consumer2/"]
RUN dotnet restore "SWE_Consumer2/SWE_Consumer2.csproj"
COPY . .
WORKDIR "/src/SWE_Consumer2"
RUN dotnet build "SWE_Consumer2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SWE_Consumer2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SWE_Consumer2.dll"]
