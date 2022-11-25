SET mypath=%~dp0
rd /S /Q "%mypath%pgsql"

tar -xf "%mypath%pgsql.zip" -C "%mypath% "

icacls %mypath% /grant SYSTEM:(OI)(CI)F /T

pgsql\bin\pg_ctl start -D pgsql\data 
set /p password=Input the password: 
pgsql\bin\psql -U postgres -d monitor -c "alter user postgres with password '%password%'" 
pgsql\bin\pg_ctl stop -D pgsql\data 
powershell -Command "(gc pgsql\data\pg_hba.conf) -replace 'trust', 'md5' | Out-File -encoding ASCII pgsql\data\pg_hba.conf"

"%mypath%\nssm" remove TimeControl"

"%mypath%\nssm" install TimeControl "%mypath%Monitor.exe"
"%mypath%\nssm" set TimeControl AppDirectory "%mypath% "
"%mypath%\nssm" set TimeControl DisplayName TimeControl
"%mypath%\nssm" set TimeControl Start SERVICE_AUTO_START
"%mypath%\nssm" set TimeControl Type SERVICE_INTERACTIVE_PROCESS
"%mypath%\nssm" set TimeControl ObjectName LocalSystem

Pause TimeControl installed. Press a key ...