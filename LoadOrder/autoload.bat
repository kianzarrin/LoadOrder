@echo off
setlocal enabledelayedexpansion
echo =============================================

REM TODO: change to match your paths:
set APP_DATA="C:\Users\dell\AppData\Local\Colossal Order\Cities_Skylines"
set GAME_DATA="C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data"
set OUTPUT_LOG=%GAME_DATA%\output_log.txt
set STEAM_PATH="C:\Program Files (x86)\Steam"
set TEST_DIR=%APP_DATA%\AUTO_TESTS
set BACUP_DIR=%TEST_DIR%\BACKUP
set OUTPUT_PATTERN="LoadOrderMod:GAME LOADING HAS FINISHED"
echo %APP_DATA%
echo %GAME_DATA%
echo %OUTPUT_LOG%
echo %STEAM_PATH%
echo %TEST_DIR%
echo %BACUP_DIR%
echo %APP_DATA%
echo %OUTPUT_PATTERN%

REM preparation
TASKKILL /IM Cities.exe /F > nul 2>&1
mkdir %TEST_DIR% > nul 2>&1
mkdir %BACUP_DIR% > nul 2>&1
xcopy /y %APP_DATA%\LoadOrder.cgs %BACUP_DIR% 
xcopy /y %APP_DATA%\userGameState.cgs %BACUP_DIR% 

REM TODO: create directories in AUTO_TESTS like CONFIG1 CONFIG2 CONFIG3 ... and put LoadOrder.cgs and userGameState.cgs in them
FOR /L %%s IN (1,1,3) DO (
	set CONFIG_DIR=!TEST_DIR!\CONFIG%%s
	set RESULTS_DIR=!TEST_DIR!\RESULTS%%s
	echo loop %%s
	echo !CONFIG_DIR!
	echo !RESULTS_DIR!

	xcopy /y !CONFIG_DIR!\*  %APP_DATA%
	del %OUTPUT_LOG%
	%STEAM_PATH%\steam -applaunch 255710 --continuelastsave
	REM echo OUTPUT_PATTERN >  %OUTPUT_LOG% REM for testing

	echo waiting for game loading to finsih ...
	call :waitforgame
	echo Gamle Loading Finished
	TASKKILL /IM Cities.exe /F
	
	REM backup output logs and game state.
	rmdir /S /Q !RESULTS_DIR! > nul 2>&1 REM clear result directory if it exists.
	mkdir !RESULTS_DIR!
	xcopy /y %APP_DATA%\LoadOrder.cgs !RESULTS_DIR!
	xcopy /y %APP_DATA%\userGameState.cgs !RESULTS_DIR!
	xcopy /y %OUTPUT_LOG% !RESULTS_DIR!
	xcopy /y %GAME_DATA%\Logs\*.log !RESULTS_DIR!	
)

REM resatore state
xcopy /y %BACUP_DIR%\* %APP_DATA%
set STEAM_PATH=
set GAME_DATA=
set OUTPUT_LOG=
set APP_DATA=
set TEST_DIR=
set BACUP_DIR=
set OUTPUT_PATTERN=
set CONFIG_DIR=
set RESULTS_DIR=

exit /b

:waitforgame
	> nul timeout /t:1
	findstr /c:%OUTPUT_PATTERN% %OUTPUT_LOG%
	IF "%ERRORLEVEL%"=="1" (GOTO waitforgame)
exit /b

