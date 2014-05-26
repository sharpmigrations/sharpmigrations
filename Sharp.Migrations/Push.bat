call nuget pack -Prop Configuration=Release
call nuget push *.nupkg 
del *.nupkg