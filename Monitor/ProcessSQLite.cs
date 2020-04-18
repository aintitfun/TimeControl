using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using Common;

namespace Monitor
{
    class ProcessSQLite
    {

        private string strSQLitePath=AppDomain.CurrentDomain.BaseDirectory+"sqlite.db";
        private Logger logger;

        public void CheckAndRecreateTables()
        {
            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {

                sqlite.Open();
                cmd.CommandText = $@"select count(*) from SQLITE_MASTER where name like '%app%' and type like 'table'";
                if (System.Convert.ToInt32(cmd.ExecuteScalar()) < 3)
                {
                    using (var cmdCreate = new SQLiteCommand(sqlite))
                    {
                        cmdCreate.CommandText = "drop table if exists  apps; " +
                            "drop table if exists  daily_apps; " +
                            "drop table if exists  hist_apps; " +
                            "CREATE TABLE apps (name text primary key, max_time int); " +
                            "CREATE TABLE daily_apps (pid integer,app text,start_time datetime,end_time datetime,primary key(pid, app));" +
                            "CREATE TABLE hist_apps (pid integer,app text,start_time datetime,end_time datetime);";
                        cmdCreate.ExecuteNonQuery();
                    }
                }
            }

        }

        /// <summary>
        /// closes previous not closed (endtime null) apps and move rows to hist table
        /// </summary>
        /// <value></value>
        public void HistApps(){
            //SQLiteConnection sqlite;
            //SQLiteDataAdapter ad;
            //SQLiteCommand cmd;

            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
            
                sqlite.Open();
                cmd.CommandText = $@"update daily_apps set end_time=(select max(start_time) from daily_apps) where end_time is null";
                cmd.ExecuteNonQuery();
            }
            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                sqlite.Open();
                cmd.CommandText = $@"insert into  hist_apps select * from daily_apps";
                cmd.ExecuteNonQuery();
            }
            //delete until one month
            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {

                sqlite.Open();
                cmd.CommandText = $@"delete from hist_apps where start_time <datetime('now','-1 month')";
                cmd.ExecuteNonQuery();
            }
            ClearTable("daily_apps");
        }

        
        /// <summary>
        /// Deletes the specified table
        /// </summary>
        /// <param name="strTable"></param> Table to delete
        public void ClearTable(string strTable){
            SQLiteConnection sqlite;
            SQLiteDataAdapter ad;
            using (sqlite = new SQLiteConnection("Data Source="+strSQLitePath))
            {
            
                SQLiteCommand cmd;
                sqlite.Open();  
                cmd = sqlite.CreateCommand();
                cmd.CommandText = $@"delete from {strTable}";
                //using (ad = new SQLiteDataAdapter(cmd))
                //{
                    cmd.ExecuteNonQuery();

                //}
            
            }
        }

        /// <summary>
        /// Inserts on a processpersist object all the records from daily_apps table
        /// </summary>
        /// <param name="p"></param>
        public void GetDailyApps(ref List<ProcessesPersist> lp){
            SQLiteConnection sqlite;
            SQLiteCommand cmd;

            using (sqlite = new SQLiteConnection("Data Source="+strSQLitePath))
            {
         
                SQLiteDataReader dr;
                sqlite.Open();  
                cmd = sqlite.CreateCommand();
                cmd.CommandText = $@"select pid,app from daily_apps where end_time is null";
                dr = cmd.ExecuteReader();

                while (dr.Read()){
                    lp.Add(new ProcessesPersist(dr.GetInt32(0),dr.GetString(1)));
                }
            
            }

        }
/// <summary>
/// Adds an app to the list of timed/disabled apps
/// </summary>
/// <param name="strAppName"></param>
/// <param name="nMaxTime"></param>
        public bool AddApplication(string strAppName, int nMaxTime)
        {

            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                sqlite.Open();  //Initiate connection to the db
                cmd.CommandText = $@"insert into apps (name,max_time) values ('{strAppName}','{nMaxTime}')";
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    if (e.ErrorCode == (int)SQLiteErrorCode.Busy)
                    {
                        //logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                        return false;
                    }
                    return true;

                }
                

             
            }
        }

        /// <summary>
        /// Remove app from the list of timed/disabled apps
        /// </summary>
        /// <param name="strAppName"></param>
        public bool RemoveApplication(string strAppName)
        {


            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                            
                sqlite.Open();  //Initiate connection to the db
                cmd.CommandText = $@"delete from apps where name ='{strAppName}'";

                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    if (e.ErrorCode == (int)SQLiteErrorCode.Busy)
                    {
                        //logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                        return false;
                    }
                    return true;
                }
            }
            
        }

        /// <summary>
        ///  Get a list of the timed/disabled apps
        /// </summary>
        /// <returns></returns>
        public List<AppsPersist> GetApps(){

            List<AppsPersist> l=new List<AppsPersist>();

            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                SQLiteDataReader dr;

                
                sqlite.Open();  //Initiate connection to the db
                cmd.CommandText = $@"select name, max_time from apps";

                try
                {

                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        l.Add(new AppsPersist(dr.GetString(0), dr.GetInt32(1)));
                    }
                }
                catch (SQLiteException e)
                {
                    if (e.ErrorCode == (int)SQLiteErrorCode.Busy)
                    {
                        logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                    }
                    return l;

                }
            }
            return l;
        }

        /// <summary>
        /// Get some app minutes active during current day
        /// </summary>
        /// <param name="strAppName"></param>
        /// <returns></returns>
        public int GetActiveMinutes(string strAppName){

            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                SQLiteDataReader dr;
                
                sqlite.Open();  //Initiate connection to the db
                /*cmd.CommandText = $@"select ifnull(sum(minutes),0) from (
                                        select ifnull(strftime('%M',end_time),strftime('%M','now')) - strftime('%M',start_time) as minutes from daily_apps where app ='{strAppName}'
                                        union all 
                                        select ifnull(strftime('%M',end_time),strftime('%M','now')) - strftime('%M',start_time) as minutes from hist_apps where app ='{strAppName}' and start_time>date())";*/

                cmd.CommandText = $@"select ifnull(sum(minutes),0) from(
                                        select cast((
                                        ifnull(julianday(end_time), julianday(datetime('now', 'localtime'))) - julianday(start_time)
                                        ) * 24 * 60 AS INTEGER) as minutes from daily_apps da where app ='{strAppName}'
                                        union all
                                        select cast((
                                        ifnull(julianday(end_time), julianday(datetime('now', 'localtime'))) - julianday(start_time)
                                        ) * 24 * 60 AS INTEGER) as minutes from hist_apps da where app ='{strAppName}' and start_time > date()
                                    )";

                dr = cmd.ExecuteReader();
                while (dr.Read())
                    return dr.GetInt32(0);
                
            }
            return 0;
        }

       
        public List<AppsPersist> GetCurrentDayAppUsage(){

            List<AppsPersist> lap=new List<AppsPersist>();

            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                SQLiteDataReader dr;
                
                sqlite.Open();  //Initiate connection to the db
                cmd.CommandText = $@"select app,sum(ifnull(minutes,0)) from (
	                                        select app,cast((
	                                        ifnull(julianday(end_time), julianday(datetime('now', 'localtime'))) - julianday(start_time)
	                                        ) * 24 * 60 AS INTEGER) as minutes from daily_apps 
	                                        union all
	                                        select app,cast((
	                                        ifnull(julianday(end_time), julianday(datetime('now', 'localtime'))) - julianday(start_time)
	                                        ) * 24 * 60 AS INTEGER) as minutes from hist_apps where start_time > date()
                                        )
                                        group by app order by 2 desc limit 10";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                    lap.Add(new AppsPersist(dr.GetString(0),dr.GetInt32(1)));
                return lap;
                
            }
        }

        /// <summary>
        /// This method updates or inserts the app on db.false Also updates the start and end Time
        /// </summary>
        /// <param name="strApp"></param>
        /// <param name="nPid"></param>
        /// <param name="dtEndTime"></param>
        public void UpdateApp(string strApp, int nPid)
        {

            using (var sqlite = new SQLiteConnection("Data Source=" + strSQLitePath))
            using (var cmd = new SQLiteCommand(sqlite))
            {
                
                sqlite.Open();  //Initiate connection to the db
                cmd.CommandText = $@"INSERT INTO daily_apps(app,start_time,pid) VALUES('{strApp}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',{nPid})";
                try 
                {
                    cmd.ExecuteNonQuery();
                }//si no es una nueva app saltará al catch

                catch (SQLiteException e)
                {
                    if (e.ErrorCode == (int)SQLiteErrorCode.Busy)
                    {
                        logger.Log($@"{DateTime.Now} [LOCK]: {cmd.CommandText}");
                        //return false;
                    }
                    using (var cmdUpd = new SQLiteCommand(sqlite))
                    {
                        cmdUpd.CommandText = $@"update daily_apps set end_time='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where app='{strApp}' and pid={nPid}";
                        cmdUpd.ExecuteNonQuery();
                    }
                }
                
            }
        }
    }
}
