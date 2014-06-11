cd..
copy .\nuget\Sharp.Data.nuspec

call nuget pack -Prop Configuration=Release
copy *.nupkg C:\Users\Andre\AppData\Local\NuGet\Cache
rem call nuget push *.nupkg

del Sharp.Data.nuspec
del *.nupkg
cd nuget