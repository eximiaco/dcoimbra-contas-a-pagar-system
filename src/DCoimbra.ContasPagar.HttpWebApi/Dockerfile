FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/DCoimbra.ContasPagar.HttpWebApi/DCoimbra.ContasPagar.HttpWebApi.csproj", "DCoimbra.ContasPagar.HttpWebApi/"]
RUN dotnet restore "src/DCoimbra.ContasPagar.HttpWebApi/DCoimbra.ContasPagar.HttpWebApi.csproj"
COPY . .
WORKDIR "/src/DCoimbra.ContasPagar.HttpWebApi"
RUN dotnet build "DCoimbra.ContasPagar.HttpWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DCoimbra.ContasPagar.HttpWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DCoimbra.ContasPagar.HttpWebApi.dll"]
