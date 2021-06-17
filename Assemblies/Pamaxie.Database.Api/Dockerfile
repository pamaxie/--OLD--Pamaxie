#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Pamaxie.Api/Pamaxie.Api.csproj", "Pamaxie.Api/"]
COPY ["Pamaxie.Database/Pamaxie.Database.csproj", "Pamaxie.Database/"]
COPY ["Pamaxie.Main/Pamaxie.Main.csproj", "Pamaxie.Main/"]
COPY ["PamaxieML.Model/PamaxieML.Model.csproj", "PamaxieML.Model/"]
COPY ["Pamaxie.Extensions/Pamaxie.Extensions.csproj", "Pamaxie.Extensions/"]
RUN dotnet restore "Pamaxie.Api/Pamaxie.Api.csproj"
COPY . .
WORKDIR "/src/Pamaxie.Api"
RUN dotnet build "Pamaxie.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pamaxie.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pamaxie.Api.dll"]