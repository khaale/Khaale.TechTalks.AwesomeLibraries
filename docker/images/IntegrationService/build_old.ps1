$homeDir = (Resolve-Path .\).Path
$distPath = "dist"
New-Item $distPath -ItemType directory -Force

#Downloading dotnet framework
$dotnetPath =  "https://go.microsoft.com/fwlink/?LinkID=825882"
wget $dotnetPath -OutFile $distPath/dotnet.zip
Expand-Archive -Path $distPath/dotnet.zip -DestinationPath $distPath/dotnet -Force ;

#Building application
$projectPath = "..\..\..\src\IntegrationServiceCore"
New-Item $distPath\app -ItemType directory -Force
$outputPath = (Resolve-Path $distPath\app).Path

&cd $projectPath 
&dotnet publish -o $outputPath
&cd $homeDir

&docker build -t khaale-awesome_libs-integration_service .
&docker tag khaale-awesome_libs-integration_service:latest khaale-awesome_libs-integration_service:0.0.1

rm $distPath -Recurse -Force
