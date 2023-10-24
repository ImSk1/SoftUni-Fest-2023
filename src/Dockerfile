FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5102
EXPOSE 7215

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SoftwareFest/SoftwareFest.csproj", "SoftwareFest/"]
RUN dotnet restore "SoftwareFest/SoftwareFest.csproj"
COPY . .
WORKDIR "/src/SoftwareFest"
RUN dotnet build "SoftwareFest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoftwareFest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareFest.dll"]