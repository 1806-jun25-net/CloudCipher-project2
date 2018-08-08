FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY RestaurantAPI.ConsoleTest/*.csproj ./RestaurantAPI.ConsoleTest/
COPY RestaurantAPI.API/*.csproj ./RestaurantAPI.API/
COPY RestaurantAPI.Library/*.csproj ./RestaurantAPI.Library/
COPY RestaurantAPI.Testing/*.csproj ./RestaurantAPI.Testing/
COPY RestaurantAPI.Data/*.csproj ./RestaurantAPI.Data/
RUN dotnet restore

# copy everything else and build app
COPY . ./
WORKDIR /app/RestaurantAPI.API
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/RestaurantAPI.API/out ./
ENTRYPOINT ["dotnet", "RestaurantAPI.API.dll"]