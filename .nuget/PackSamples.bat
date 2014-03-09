cd ..
for /f %%d in ('dir /od /a-d /b /s Test.Sample.*.csproj') do .nuget\Nuget.exe pack %%d -OutputDirectory Run00.SemVer.Cecil.IntegrationTest\Artifacts
cd .nuget
