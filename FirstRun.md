#Install self cert:
WindowsPowerShell as Admin: New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "cert:\LocalMachine\My"
mmc.exe File -> Add or Remove Snap-ins -> Certificates -> Add -> Computer account -> Local computer. Click Finish.
copy localhost cert from personal to root trusted

in browser enable localhost cert: edge://flags/#allow-insecure-localhost

to debug, ensure right helper by downloading BlazorDebugProxy nuget with latest version corresponding with installed sdk
https://www.nuget.org/packages/Microsoft.AspNetCore.Components.WebAssembly.DevServer/


window.zoomLevel set in prefs!