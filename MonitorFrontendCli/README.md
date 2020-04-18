# MonitorFrontendCli

Simple monitor to control consumed time of an app.

This is the CLI part. Note that MonitorFrontendCli binary needs Common.dll

Examples:

        MonitorFrontendCli -list myserver

List apps on the monitor installed on myserver

        MonitorFrontendCli -add myserver myapp 120

Grants to execute 120 minutes per day to myapp application on myserver
  
        MonitorFrontendCli -remove myserver myapp

Removes myapp from the list of monitored apps. So myapp could be run on unlimited time
  

        MonitorFrontendCli -stats myserver

Show minutes cosumed on the top 10 apps
  
  
Note the Cli only uses network names at this moment.
