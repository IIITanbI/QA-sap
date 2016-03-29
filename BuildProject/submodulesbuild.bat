echo "building submodules"
SET msb="C:\Program Files (x86)\MSBuild\14.0\Bin"
echo %msb%
%msb%\msbuild.exe ..\QA.AutomatedMagic\QA.AutomatedMagic.sln
..\QA.AutomatedMagic.Managers\nuget.exe restore ..\QA.AutomatedMagic.Managers\QA.AutomatedMagic.Managers.sln
%msb%\msbuild.exe ..\QA.AutomatedMagic.Managers\QA.AutomatedMagic.Managers.sln