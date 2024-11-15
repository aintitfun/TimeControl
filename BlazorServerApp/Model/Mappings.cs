using FluentNHibernate.Mapping;

namespace BlazorServerApp.Model
{
    public class AppMap : ClassMap<App>
    {
        public AppMap()
        {
            Table("apps");
            CompositeId()
                .KeyProperty(x => x.Name, "name")
                .KeyProperty(x => x.Username, "username")
                .KeyProperty(x => x.DayOfTheWeek, "day_of_the_week");
            Map(x => x.MaxTime, "max_time");
        }
    }

    public class DailyAppMap : ClassMap<DailyApp>
    {
        public DailyAppMap()
        {
            Table("daily_apps");
            CompositeId()
                .KeyProperty(x => x.Pid, "pid")
                .KeyProperty(x => x.App, "app");
            Map(x => x.Username, "username");
            Map(x => x.StartTime, "start_time");
            Map(x => x.EndTime, "end_time");
        }
    }

    public class HistAppMap : ClassMap<HistApp>
    {
        public HistAppMap()
        {
            Table("hist_apps");
            Id(x => x.Pid, "pid");
            Map(x => x.App, "app");
            Map(x => x.Username, "username");
            Map(x => x.StartTime, "start_time");
            Map(x => x.EndTime, "end_time");
        }
    }

    public class ActiveTimeMap : ClassMap<ActiveTime>
    {
        public ActiveTimeMap()
        {
            Table("activetime");
            CompositeId()
                .KeyProperty(x => x.Username, "username")
                .KeyProperty(x => x.DayOfTheWeek, "day_of_the_week");
            Map(x => x.MaxTime, "max_time");
            Map(x => x.LastTimeConnected, "last_time_connected");
            Map(x => x.SecondsToday, "seconds_today");
        }
    }

    public class LogoutMap : ClassMap<Logout>
    {
        public LogoutMap()
        {
            Table("logouts");
            CompositeId()
                .KeyProperty(x => x.Username, "username")
                .KeyProperty(x => x.DayOfTheWeek, "day_of_the_week");
            Map(x => x.HourMin, "hour_min");
        }
    }

    public class LoginMap : ClassMap<Login>
    {
        public LoginMap()
        {
            Table("logins");
            CompositeId()
                .KeyProperty(x => x.Username, "username")
                .KeyProperty(x => x.DayOfTheWeek, "day_of_the_week");
            Map(x => x.HourMin, "hour_min");
        }
    }

    public class LogoutNowMap : ClassMap<LogoutNow>
    {
        public LogoutNowMap()
        {
            Table("logoutsnow");
            Id(x => x.Username, "username");
            Map(x => x.Day, "day");
        }
    }
}

