@echo off

set APP_DATA="C:\Users\dell\AppData\Local\Colossal Order\Cities_Skylines"
set GAME_DATA="C:\Program Files (x86)\Steam\steamapps\common\Cities_Skylines\Cities_Data"
set OUTPUT_LOG=%GAME_DATA%\output_log.txt"
set STEAM_PATH="C:\Program Files (x86)\Steam"
set OUTPUT_PATTERN="LoadOrderMod:GAME LOADING HAS FINISHED"
for %%s in (1,1,3) DO (
	TASKKILL /IM Cities.exe /F
	del %OUTPUT_LOG%

	copy /y %APP_DATA%\LoadOrder%%s.cgs %APP_DATA%\LoadOrder.cgs > nul
	copy /y %APP_DATA%\userGameState%%s.cgs %APP_DATA%\userGameState.cgs > nul
	%STEAM_PATH%\steam steam://rungameid/255710 --continuelastsave

	echo waiting for game loading to finsih ...
	:gameloop
		findstr /c:%OUTPUT_PATTERN% %OUTPUT_LOG% >nul	
		IF NOT ERRORLEVEL 1 GOTO endloop
		timeout /t 1 /nobreak > nul REM wait a second
		goto gameloop
	:endloop
	
	echo Gamle Loading Finished!
	TASKKILL /IM Cities.exe /F
	
	REM backup output logs and game state.
	set RESULTS_DIR=%GAME_DATA%\AUTO_TEST_RESULTS%%s
	mkdir %RESULTS_DIR%
	copy /y %APP_DATA%\LoadOrder.cgs %RESULTS_DIR%
	copy /y %APP_DATA%\userGameState.cgs %RESULTS_DIR%
	copy /y %OUTPUT_LOG% %RESULTS_DIR%
	copy /y %GAME_DATA%\Logs\*.log %RESULTS_DIR%	
)
