FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY AdventOfCode2019/AdventOfCode2019.csproj AdventOfCode2019/
RUN dotnet restore "AdventOfCode2019/AdventOfCode2019.csproj"
COPY . .
WORKDIR "/src/AdventOfCode2019"
RUN dotnet build "AdventOfCode2019.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdventOfCode2019.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdventOfCode2019.dll"]
