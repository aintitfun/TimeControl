SET mypath=%~dp0
rd /S /Q "%mypath%pgsql"

tar -xf "%mypath%pgsql.zip" -C "%mypath% "

icacls %mypath% /grant SYSTEM:(OI)(CI)F /T

"%mypath%\nssm" remove TimeControl"

"%mypath%\nssm" install TimeControl "%mypath%Monitor.exe"
"%mypath%\nssm" set TimeControl AppDirectory "%mypath% "
"%mypath%\nssm" set TimeControl DisplayName TimeControl
"%mypath%\nssm" set TimeControl Start SERVICE_AUTO_START
"%mypath%\nssm" set TimeControl Type SERVICE_INTERACTIVE_PROCESS
"%mypath%\nssm" set TimeControl ObjectName LocalSystem

Pause TimeControl installed. Press a key ...