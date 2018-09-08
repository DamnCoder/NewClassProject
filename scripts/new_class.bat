@echo off

cd..
set TEST_DIR=.\TestFolder
set FILE_OUTPUT=%TEST_DIR%
set NEWCLASSBIN=.\Build\NewClass.exe
set NEWCLASSBUILDSCRIPT=.\scripts\build.bat
set TEMPLATES_PATH=.\Templates

set FILE_NAME=test
set PROJECT_NAME=Gander
set CLASS_NAME=CTest
set NAMESPACE=dc
set FOLDER=test
set SRC_FOLDER=Private
set HEADER_FOLDER=Public
set AUTHOR="Jorge Lopez Gonzalez"

echo Build NewClassProject just if needed

call %NEWCLASSBUILDSCRIPT%

echo Does the test folder %TEST_DIR% exists?

if not exist %TEST_DIR% (
	echo %TEST_DIR% does not exists so we create it
	mkdir %TEST_DIR%
	mkdir %INCLUDE_FOLDER%
	mkdir %SOURCE_FOLDER%
)

echo Create the class %CLASS_NAME% on %FOLDER% directory

%NEWCLASSBIN% %TEMPLATES_PATH% %FILE_OUTPUT% %FILE_NAME% %PROJECT_NAME% %AUTHOR% %CLASS_NAME% %NAMESPACE% %FOLDER% %SRC_FOLDER% %HEADER_FOLDER%

exit