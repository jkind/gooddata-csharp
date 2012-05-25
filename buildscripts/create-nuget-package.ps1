$solutionName = "GoodData"
$projectName = "GoodDataService"
$clean = "msbuild " + $solutionName + ".sln /t:Clean"
$build = "msbuild " + $solutionName + ".sln /t:Build /p:Configuration=Release"
$nupack = "nuget pack " + $projectName + "/" + $projectName + ".csproj -Symbols" 

write("Commands......")
write($basedir)
write($clean)
write($build)
write($nupack)

cd ..

Invoke-Expression $clean
Invoke-Expression $build
Invoke-Expression $nupack

cd buildscripts