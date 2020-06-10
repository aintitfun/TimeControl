using System.Xml;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.conn;
using Npgsql;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using Common;

namespace Monitor
{
    class ProcessSQL
    {

        private string strconnPath=AppDomain.CurrentDomain.BaseDirectory+"conn.db";
        private Logger logger;
        private string connString;
        public ProcessSQL()
        {
            connString="Host=127.0.0.1;Username=postgres;Password=postgres01;Database=monitor;";
        }
        public void CheckAndRecreateTables()
        {
            int AppTables;
            using (var vConn = new NpgsqlConnection(connString)){
                vConn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select count(*) from pg_tables where tablename like '%app%'",vConn))
                {
                    AppTables=System.Convert.ToInt32(cmd.ExecuteScalar());
                }
                                
                //cmd.CommandText = $@"select count(*) from pg_tables where tablename like '%app%'";
                if (AppTables > 3)
                    return;
                
                using (NpgsqlCommand cmdCreate = new NpgsqlCommand("drop table if exists  apps; " +
                        " drop table if exists  daily_apps; " +
                        " drop table if exists  hist_apps; " +
                        " drop table if exists  logouts; " +
                        " CREATE TABLE apps (name text , username text,max_time int, primary key (name,username)); " +
                        " CREATE TABLE daily_apps (pid int,app text,username text,start_time timestamp,end_time timestamp,primary key(pid, app));" +
                        " CREATE TABLE hist_apps (pid int,app text,username text,start_time timestamp,end_time timestamp);"+
                        " create table logouts (username text primary key, hour_min text);",vConn)){
                    cmdCreate.ExecuteNonQuery();
                }
                
            }

        }

        /// <summary>
        /// closes previous not closed (endtime null) apps and move rows to hist table
        /// </summary>
        /// <value></value>
        public void HistApps(){
            //NpgsqlConnection conn;
            //connDataAdapter ad;
            //NpgsqlCommand cmd;

            using (var conn = new NpgsqlConnection(connString)){
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand("update daily_apps set end_time=(select max(start_time) from daily_apps) where end_time is null",conn))
                {
                
                    cmd.ExecuteNonQuery();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand("insert into  hist_apps select * from daily_apps",conn))
                {
                    cmd.ExecuteNonQuery();
                }
                //delete until one month
                using (NpgsqlCommand cmd = new NpgsqlCommand("delete from hist_apps where start_time < now()- interval '1 month'",conn))
                {

                    cmd.ExecuteNonQuery();
                }
                ClearTable("daily_apps");
            }
        }

        
        /// <summary>
        /// Deletes the specified table
        /// </summary>
        /// <param name="strTable"></param> Table to delete
        public void ClearTable(string strTable){
            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"truncate table {strTable}",conn))
                    cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds on a processpersist object all the records from daily_apps table
        /// </summary>
        /// <param name="p"></param>    
        public void GetDailyApps(ref List<ProcessesPersist> lp){
            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand("select pid,app,coalesce(username,'no_username') from daily_apps where end_time is null",conn))
                {
                    NpgsqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read()){
                        lp.Add(new ProcessesPersist(dr.GetInt32(0),dr.GetString(1),dr.GetString(2)));
                    }
                }
            }
        }

        /// <summary>
        /// Adds an app to the list of timed/disabled apps
        /// </summary>
        /// <param name="strAppName"></param>
        /// <param name="nMaxTime"></param>
        public bool AddApplication(string strAppName, string userName,int nMaxTime)
        {
            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"insert into apps (name,username,max_time) values ('{strAppName}','{userName}','{nMaxTime}')",conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (NpgsqlException e)
                    {
                        if (e.Message.Contains(Npgsql.PostgresErrorCodes.LockNotAvailable))
                        {
                            //logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                            return false;
                        }
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Remove app from the list of timed/disabled apps
        /// </summary>
        /// <param name="strAppName"></param>
        public bool RemoveApplication(string strAppName)
        {


            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"delete from apps where name ='{strAppName}'",conn))
                {
                try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (NpgsqlException e)
                    {
                        if (e.Message.Contains(Npgsql.PostgresErrorCodes.LockNotAvailable))
                        {
                            //logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                            return false;
                        }
                        return true;
                    }
                }
            }
            
        }

        /// <summary>
        ///  Get a list of the timed/disabled apps
        /// </summary>
        /// <returns></returns>
        public List<AppsPersist> GetApps(){

            List<AppsPersist> l=new List<AppsPersist>();

            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand("select name,username, max_time from apps",conn))
                {
                    NpgsqlDataReader dr;
                    try
                    {
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            l.Add(new AppsPersist(dr.GetString(0), dr.GetString(1), dr.GetInt32(2) ));
                        }
                    }
                    catch (NpgsqlException e)
                    {
                        if (e.Message.Contains(Npgsql.PostgresErrorCodes.LockNotAvailable))
                        {
                            //logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                        }
                        return l;

                    }
                }
            }
            return l;
        }

        /// <summary>
        /// Get some app minutes active during current day
        /// </summary>
        /// <param name="strAppName"></param>
        /// <returns></returns>
        public int GetActiveMinutes(string strAppName, string strUser){

            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"select coalesce(sum(minutes)::integer,0) from(
                                        select
                                        extract (epoch from (coalesce(end_time, now()) - start_time))/60 as minutes
                                        from daily_apps da where app ='{strAppName}' and username='{strUser}'
                                        union all
                                        select 
                                        extract (epoch from (coalesce(end_time, now()) - start_time))/60 as minutes
                                        from hist_apps da where app ='{strAppName}' and username='{strUser}' and start_time >date_trunc('day',now())
                                    )t",conn))
                {
                    NpgsqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        return dr.GetInt32(0);
                
                }
            }
            return 0;
        }

       
        public List<AppsPersist> GetCurrentDayAppUsage(){

            List<AppsPersist> lap=new List<AppsPersist>();

            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"select app,username,sum(coalesce(minutes,0))::integer from (
	                                        select app,username,
	                                        extract(epoch from (coalesce(end_time, now()) - start_time))/60 as minutes 
                                            from daily_apps 
	                                        union all
	                                        select app,username,
	                                        extract(epoch from (coalesce(end_time, now()) - start_time))/ 60 as minutes 
                                            from hist_apps where start_time > now()
                                        )t
                                        group by app,username order by 3 desc limit 10",conn))
                {
                    NpgsqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        lap.Add(new AppsPersist(dr.GetString(0),dr.GetString(1),dr.GetInt32(2)));
                    return lap;
                }
            }
        }

        public List<AppsPersist> GetConfiguredLogouts()
        {
            
            List<AppsPersist> lap=new List<AppsPersist>();

            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"select username,hour_min from logouts;",conn))
                {
                    NpgsqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        lap.Add(new AppsPersist(null,dr.GetString(0),System.Convert.ToInt32(dr.GetString(1))));
                    return lap;
                }
            }
        }

        public bool AddLogout(string userName, int hour_min)
        {
            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"insert into logouts values ('{userName}',{hour_min});",conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (NpgsqlException e)
                    {
                        //logger.Log($@"{DateTime.Now} [ERROR]: inserting logout for {userName}");
                    }
                    return false;
                }
            }
        }
        public bool RemoveLogout(string userName)
        {
            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"delete from logouts where username ='{userName}';",conn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (NpgsqlException e)
                    {
                        //logger.Log($@"{DateTime.Now} [ERROR]: Removing logout {userName}");
                    }
                    return false;
                }
            }
        }
        /// <summary>
        /// This method updates or inserts the app on db.false Also updates the start and end Time
        /// </summary>
        /// <param name="strApp"></param>
        /// <param name="nPid"></param>
        /// <param name="dtEndTime"></param>
        public void UpdateApp(string strApp, string userName,int nPid)
        {

            using (var conn = new NpgsqlConnection(connString))
            {                
                conn.Open();  
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"INSERT INTO daily_apps(app,username,start_time,pid) VALUES('{strApp}','{userName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',{nPid})",conn))
                {
                    try 
                    {
                        cmd.ExecuteNonQuery();
                    }//si no es una nueva app saltará al catch

                    catch (NpgsqlException e)
                    {
                        if (e.Message.Contains(Npgsql.PostgresErrorCodes.LockNotAvailable))
                        {
                            //logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                            //return false;
                        }
                        using (NpgsqlCommand cmdUpd = new NpgsqlCommand($@"update daily_apps set end_time='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where app='{strApp}' and pid={nPid}",conn))
                        {
                            cmdUpd.ExecuteNonQuery();
                        }
                    }
                }                
            }
        }

        public List<string> GetUsersToLogOut()
        {
            List<string> usersToLogOut=new List<string>();
            using (var vConn = new NpgsqlConnection(connString))
            {
                vConn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($@"select username from logouts "+
                    " where now()>date_trunc('day',now() )+(substring(hour_min from 1 for 2)||' hour')::interval+(substring(hour_min from 3 for 2)||' minutes')::interval;",vConn))
                {
                    NpgsqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        usersToLogOut.Add(dr.GetString(0));
                    return usersToLogOut;
                }
            }
        }
    }
}
