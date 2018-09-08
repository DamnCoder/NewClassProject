@echo off

cd..

set TEST_DIR=.\TestFolder
set BUILD_DIR=.\Build

rmdir %TEST_DIR% /s /q
rmdir %BUILD_DIR% /s /q

exit