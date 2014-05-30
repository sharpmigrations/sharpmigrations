del tools /s /q
mkdir tools
copy ..\SharpMigrator\bin\Release\*.exe .\tools
copy ..\SharpMigrator\bin\Release\*.dll .\tools
copy ..\SharpMigrator\bin\Release\*.config .\tools
rem copy init.ps1 .\tools
rem copy SharpMigrator.psm1 .\tools
call nuget pack -Prop Configuration=Release
rem copy *.nupkg C:\Users\Andre\AppData\Local\NuGet\Cache
call nuget push *.nupkg 
del *.nupkg
del tools /s /q