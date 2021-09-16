# MonitorFrontendCli

Simple monitor to control consumed time of an app.

This is the CLI part. Note that MonitorFrontendCli binary needs Common.dll

Examples:

        MonitorFrontendCli -listapps myserver

List apps on the monitor installed on myserver for each user

        MonitorFrontendCli -addapp myserver myapp john 120 Wednesday

Grants to execute 120 minutes on Monday to myapp application on myserver to john
  
        MonitorFrontendCli -removeapp myserver myapp john Monday

Removes myapp policy from John user. So myapp could be run by John with unlimited time on Monday
  
        MonitorFrontendCli -stats myserver

Show minutes cosumed on the top 10 apps

        MonitorFrontendCli -listlogouts myserver

List logouts times for all users

        MonitorFrontendCli -addlogout myserver john 18:30

Force logout to John at 18:30


        MonitorFrontendCli -removelogout myserver john

Removes the logout policy from john

        MonitorFrontendCli.exe -addactivetime myserver john 10 Monday

Grants to John to use the pc for 10 minutes on Monday

        MonitorFrontendCli.exe -removeactivetime myserver john Monday

Remove the policy of activetime for John on Mondays. So John can now use the pc on Mondays withoout limitation

        MonitorFrontendCli.exe -listactivetime myserver john

List all activetime policies for user john

Note the Cli only uses network names at this moment to connect to server.
