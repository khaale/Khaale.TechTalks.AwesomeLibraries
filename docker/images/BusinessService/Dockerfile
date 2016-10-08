FROM microsoft/nanoserver

ADD ./dist/dotnet/ /dotnet/
ADD ./dist/app/ /app/

WORKDIR /app
EXPOSE 5000
ENTRYPOINT [ "\\dotnet\\dotnet.exe", "\\app\\BusinessServiceCore.dll" ]