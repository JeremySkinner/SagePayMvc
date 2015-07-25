@echo off
if "%1"=="PublishPackages" goto publish

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe build.proj %*
goto end

:publish
powershell.exe -noprofile ./publish-nuget-packages.ps1

goto end

:end