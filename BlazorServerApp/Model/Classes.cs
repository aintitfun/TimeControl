using System;

namespace BlazorServerApp.Model
{
    public class App
    {
        public virtual string Name { get; set; }
        public virtual string Username { get; set; }
        public virtual int MaxTime { get; set; }
        public virtual string DayOfTheWeek { get; set; }
    }

    public class DailyApp
    {
        public virtual int Pid { get; set; }
        public virtual string App { get; set; }
        public virtual string Username { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
    }

    public class HistApp
    {
        public virtual int Pid { get; set; }
        public virtual string App { get; set; }
        public virtual string Username { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
    }

    public class ActiveTime
    {
        public virtual string Username { get; set; }
        public virtual int MaxTime { get; set; }
        public virtual string DayOfTheWeek { get; set; }
        public virtual DateTime LastTimeConnected { get; set; }
        public virtual int SecondsToday { get; set; }
    }

    public class Logout
    {
        public virtual string Username { get; set; }
        public virtual string HourMin { get; set; }
        public virtual string DayOfTheWeek { get; set; }
    }

    public class Login
    {
        public virtual string Username { get; set; }
        public virtual string HourMin { get; set; }
        public virtual string DayOfTheWeek { get; set; }
    }

    public class LogoutNow
    {
        public virtual string Username { get; set; }
        public virtual DateTime Day { get; set; }
    }

}
