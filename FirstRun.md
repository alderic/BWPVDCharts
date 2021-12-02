#Install self cert:
WindowsPowerShell as Admin: New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "cert:\LocalMachine\My"
mmc.exe File -> Add or Remove Snap-ins -> Certificates -> Add -> Computer account -> Local computer. Click Finish.
copy localhost cert from personal to root trusted

in browser enable localhost cert: edge://flags/#allow-insecure-localhost

to debug, ensure right helper by downloading BlazorDebugProxy nuget with latest version corresponding with installed sdk
https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.DevServer/


window.zoomLevel set in prefs!!




Then to run it, we need a web server to host the server side of the app, and to make that easy, I’ll use the dotnet serve global tool. To install it, run:

dotnet tool install --global dotnet-serve
at which point you can start a simple web server for the files in the published directory:

pushd D:\examples\app5\bin\Release\net5.0\publish\wwwroot
dotnet serve -o