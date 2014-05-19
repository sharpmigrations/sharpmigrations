cd Sharp.Data.Nuget\Bin\
del SharpData.nupkg
rename SharpData.* SharpData.nupkg
rem attrib +r +s SharpData.nupkg
rem del SharpData*.*
rem attrib -r -s SharpData.nupkg
..\..\.nuget\NuGet.exe push SharpData.nupkg