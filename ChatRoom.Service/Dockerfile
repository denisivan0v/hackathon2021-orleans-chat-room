# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

# copy and publish app
COPY . /src
WORKDIR /src/ChatRoom.Service
RUN dotnet restore -r linux-x64
RUN dotnet publish -c release -o /app -r linux-x64 --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-focal-amd64
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./ChatRoom.Service"]