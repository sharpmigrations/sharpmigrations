cd..
del tools /s /q
mkdir tools
copy ..\SharpMigrator\bin\Release\SharpMigrator.exe .\tools
copy ..\SharpMigrator\bin\Release\*.dll .\tools
copy ..\SharpMigrator\bin\Release\SharpMigrator.exe.config .\tools

copy nuget\init.ps1 .\tools
copy nuget\SharpMigrator.psm1 .\tools

copy .\nuget\Sharp.Migrations.nuspec
call nuget pack -Prop Configuration=Release
copy *.nupkg C:\Users\Andre\AppData\Local\NuGet\Cache
rem call nuget push *.nupkg
del Sharp.Migrations.nuspec
del *.nupkg
del tools /s /q
cd nuget