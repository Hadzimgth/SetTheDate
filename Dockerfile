# Stage 1: Build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY SetTheDate/SetTheDate.csproj ./SetTheDate/
RUN dotnet restore "./SetTheDate/SetTheDate.csproj"

# Copy the rest of the project
COPY SetTheDate/ ./SetTheDate/
WORKDIR "/src/SetTheDate"

# Build the project
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "SetTheDate.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish the project
FROM build AS publish
RUN dotnet publish "SetTheDate.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app from publish stage
COPY --from=publish /app/publish .

# Expose the port your app will run on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "SetTheDate.dll"]
