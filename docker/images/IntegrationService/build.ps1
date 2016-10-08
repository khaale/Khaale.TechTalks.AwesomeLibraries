$homeDir = (Resolve-Path .\).Path
$distPath = "dist"
New-Item $distPath -ItemType directory -Force

#Building application
$projectPath = "..\..\..\src\IntegrationServiceCore"
New-Item $distPath\app -ItemType directory -Force
$outputPath = (Resolve-Path $distPath\app).Path

&cd $projectPath 
&dotnet publish -o $outputPath
&cd $homeDir

&docker build -t awl-is .
&docker tag awl-is:latest awl-is:0.0.1

rm $distPath -Recurse -Force
