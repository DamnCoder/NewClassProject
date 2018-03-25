@echo off

set bat_dir=%~dp0
set build_dir=%bat_dir%..\Build
set bin_path=%bat_dir%..\Build\NewClass.exe

set cscomp=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe
set srcs=%bat_dir%..\NewClass\*.cs

echo We need the folder: %build_dir%

if not exist %build_dir% (
	echo %build_dir% does not exists so we create it
	mkdir %build_dir%
)

echo We compile it
%cscomp% /out:%bin_path% %srcs%
echo Build finished

::exit