cd Sharp.Nuget\Bin\
del SharpMigrations.nupkg
rename SharpMigrations.* SharpMigrations.nupkg
rem attrib +r +s SharpMigrations.nupkg
rem del SharpMigrations*.*
rem attrib -r -s SharpMigrations.nupkg
..\..\.nuget\NuGet.exe push SharpMigrations.nupkg