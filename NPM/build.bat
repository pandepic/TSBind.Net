dotnet publish ../Source/TSBind.Net/TSBindDotNet.csproj -r win-x64 -c Release --output ./windows_x64
dotnet publish ../Source/TSBind.Net/TSBindDotNet.csproj -r linux-x64 -c Release --output ./linux_x64
dotnet publish ../Source/TSBind.Net/TSBindDotNet.csproj -r osx-x64 -c Release --output ./osx_x64
for /r %%i in (*.pdb) do del %%i