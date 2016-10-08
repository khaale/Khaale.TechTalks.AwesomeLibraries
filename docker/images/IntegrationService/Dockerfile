FROM microsoft/dotnet-nano

ADD ./dist/app/ /app/

WORKDIR /app
ENTRYPOINT [ "dotnet.exe", "IntegrationServiceCore.dll" ]