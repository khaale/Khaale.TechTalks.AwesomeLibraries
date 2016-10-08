$consulVersion = "0.7.0";

$consulPath = "https://releases.hashicorp.com/consul/$consulVersion/consul_" + $consulVersion + "_windows_amd64.zip";
$consulWebUiPath = "https://releases.hashicorp.com/consul/$consulVersion/consul_" + $consulVersion + "_web_ui.zip";


$distPath = "dist"
New-Item $distPath -ItemType directory -Force

wget $consulPath  -OutFile $distPath/consul.zip;
wget $consulWebUiPath  -OutFile $distPath/webui.zip ;

Expand-Archive -Path $distPath/consul.zip -DestinationPath $distPath/consul -Force ;
Expand-Archive -Path $distPath/webui.zip -DestinationPath $distPath/webui -Force ;

&docker build -t consul-nano .
&docker tag consul-nano:latest consul-nano:0.7.0

rm $distPath -Recurse -Force
