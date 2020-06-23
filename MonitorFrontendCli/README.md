# MonitorFrontendCli

Simple monitor to control consumed time of an app.

This is the CLI part. Note that MonitorFrontendCli binary needs Common.dll

Examples:

        MonitorFrontendCli -list myserver

List apps on the monitor installed on myserver for each user

        MonitorFrontendCli -add myserver myapp john 120

Grants to execute 120 minutes per day to myapp application on myserver to john
  
        MonitorFrontendCli -remove myserver myapp john

Removes myapp policy from John user. So myapp could be run by John with unlimited time
  
        MonitorFrontendCli -stats myserver

Show minutes cosumed on the top 10 apps

        MonitorFrontendCli -listlogouts myserver

List logouts times for all users

        MonitorFrontendCli -addlogout myserver john 18:30

Force logout to John at 18:30


        MonitorFrontendCli -removelogout myserver john

Removes the logout policy from john

Note the Cli only uses network names at this moment to connect to server.
