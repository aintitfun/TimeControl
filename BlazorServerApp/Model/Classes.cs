using System;

namespace BlazorServerApp.Model
{
    public class App
    {
        public virtual string Name { get; set; }
        public virtual string Username { get; set; }
        public virtual int MaxTime { get; set; }
        public virtual string DayOfTheWeek { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (App)obj;
            return Name == other.Name && Username == other.Username && DayOfTheWeek == other.DayOfTheWeek;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Name != null ? Name.GetHashCode() : 0);
                hash = hash * 23 + (Username != null ? Username.GetHashCode() : 0);
                hash = hash * 23 + (DayOfTheWeek != null ? DayOfTheWeek.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class DailyApp
    {
        public virtual int Pid { get; set; }
        public virtual string App { get; set; }
        public virtual string Username { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (DailyApp)obj;
            return Pid == other.Pid && App == other.App;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Pid.GetHashCode();
                hash = hash * 23 + (App != null ? App.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class HistApp
    {
        public virtual int Pid { get; set; }
        public virtual string App { get; set; }
        public virtual string Username { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (HistApp)obj;
            return Pid == other.Pid && App == other.App;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Pid.GetHashCode();
                hash = hash * 23 + (App != null ? App.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class ActiveTime
    {
        public virtual string Username { get; set; }
        public virtual int MaxTime { get; set; }
        public virtual string DayOfTheWeek { get; set; }
        public virtual DateTime LastTimeConnected { get; set; }
        public virtual int SecondsToday { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ActiveTime)obj;
            return Username == other.Username && DayOfTheWeek == other.DayOfTheWeek;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Username != null ? Username.GetHashCode() : 0);
                hash = hash * 23 + (DayOfTheWeek != null ? DayOfTheWeek.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class Logout
    {
        public virtual string Username { get; set; }
        public virtual string HourMin { get; set; }
        public virtual string DayOfTheWeek { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Logout)obj;
            return Username == other.Username && DayOfTheWeek == other.DayOfTheWeek;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Username != null ? Username.GetHashCode() : 0);
                hash = hash * 23 + (DayOfTheWeek != null ? DayOfTheWeek.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class Login
    {
        public virtual string Username { get; set; }
        public virtual string HourMin { get; set; }
        public virtual string DayOfTheWeek { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (Login)obj;
            return Username == other.Username && DayOfTheWeek == other.DayOfTheWeek;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Username != null ? Username.GetHashCode() : 0);
                hash = hash * 23 + (DayOfTheWeek != null ? DayOfTheWeek.GetHashCode() : 0);
                return hash;
            }
        }
    }

    public class LogoutNow
    {
        public virtual string Username { get; set; }
        public virtual DateTime Day { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (LogoutNow)obj;
            return Username == other.Username;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Username != null ? Username.GetHashCode() : 0);
                return hash;
            }
        }
    }

}
