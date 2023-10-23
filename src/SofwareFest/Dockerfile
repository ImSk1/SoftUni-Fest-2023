FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SofwareFest/SofwareFest.csproj", "SofwareFest/"]
RUN dotnet restore "SofwareFest/SofwareFest.csproj"
COPY . .
WORKDIR "/src/SofwareFest"
RUN dotnet build "SofwareFest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SofwareFest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SofwareFest.dll"]