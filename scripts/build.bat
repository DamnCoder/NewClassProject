@echo off

cd..
set directory=.\Build

echo We need the folder: %directory%

if not exist %directory% (
	echo %directory% does not exists so we create it
	mkdir %directory%
)

C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /out:.\Build\NewClass.exe .\NewClass\*.cs