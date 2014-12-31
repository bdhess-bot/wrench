@echo off
setlocal

:: Check prerequisites
set _msbuildexe="%ProgramFiles(x86)%\MSBuild\12.0\Bin\amd64\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%WinDir%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%WinDir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
if not exist %_msbuildexe% echo Error: Could not find MSBuild.exe. && exit /b 1

set _cached_nuget=%LocalAppData%\NuGet\NuGet.exe
if not exist %_cached_nuget% @powershell -NoProfile -Command "(new-object net.webclient).DownloadFile('https://nuget.org/nuget.exe', '%_cached_nuget%')"
if not exist %_cached_nuget% echo Error: Could not find nuget.exe. && exit /b 1

%_cached_nuget% restore "%~dp0src\BrowningStyle.sln" > "%~dp0build.log"
%_msbuildexe% "%~dp0src\BrowningStyle.sln" /nologo /maxcpucount /verbosity:minimal /nodeReuse:false /fileloggerparameters:Verbosity=diag;LogFile="%~dp0build.log";Append %*