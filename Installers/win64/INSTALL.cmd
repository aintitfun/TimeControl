tar -xf %cd%\pgsql.zip

nssm install TimeControl %cd%\Monitor.exe
nssm set TimeControl AppDirectory %cd%
nssm set TimeControl DisplayName TimeControl
nssm set TimeControl Start SERVICE_AUTO_START
nssm set TimeControl Type SERVICE_INTERACTIVE_PROCESS
nssm set TimeControl ObjectName LocalSystem

Pause TimeControl installed. Press a key ...