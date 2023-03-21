using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace WoW_PVP_Team_Manager.DataBase
{
    public class DataAccess
    {        
        private static readonly string _dbPath = ApplicationData.Current.LocalFolder.Path;
        private static readonly string _conString = @"Data Source=" + _dbPath + @"\DATA.db3;Version=3";

        //Load data from the database
        public static List<T> LoadData<T>(string sqlString, object parameters)
        {
            using (IDbConnection connection = new SQLiteConnection(_conString)) 
            {
                var data = connection.Query<T>(sqlString, parameters);
                return data.ToList();
            }
        }

        //Save data to the database
        public static void SaveData<T>(string sqlString, T data)
        {
            using (IDbConnection connection = new SQLiteConnection(_conString))
            {
                connection.Execute(sqlString, data);
            }
        } 

        //Checks if database exists
        public async static Task Initialize()
        {
            //If database exists exit method
            if (File.Exists(_dbPath + "\\DATA.db3")) return;  
            
            //Create the database
            await ApplicationData.Current.LocalFolder.CreateFileAsync("DATA.db3");
            string[] sqlString = new string[] 
            {
                "CREATE TABLE \"Players\" (\"Id\"INTEGER NOT NULL UNIQUE,\"Team\"TEXT NOT NULL,\"Name\"TEXT NOT NULL,\"Date\"TEXT NOT NULL,\"Rank\"INTEGER NOT NULL,\"Active\"INTEGER NOT NULL,\"Role\"TEXT NOT NULL,PRIMARY KEY(\"Id\" AUTOINCREMENT));",
                "CREATE TABLE \"Teams\" (\"Id\"INTEGER NOT NULL UNIQUE,\"Name\"TEXT NOT NULL,\"Rank\"INTEGER NOT NULL,PRIMARY KEY(\"Id\" AUTOINCREMENT));",
                "CREATE TABLE \"LogItems\" (\"Id\"INTEGER NOT NULL UNIQUE, \"TournamentId\"INTEGER NOT NULL,\"TeamId\"TEXT NOT NULL, \"TeamName\"TEXT NOT NULL,\"PlayerName\"TEXT NOT NULL,\"Role\"TEXT NOT NULL,\"Description\"TEXT NOT NULL,\"TimeStamp\"INTEGER NOT NULL,PRIMARY KEY(\"Id\" AUTOINCREMENT));",
                "CREATE TABLE \"LogLists\" (\"Id\"INTEGER NOT NULL UNIQUE, \"Title\" TEXT NOT NULL, PRIMARY KEY(\"Id\" AUTOINCREMENT));"
            };
            
            //Execute
            using (SQLiteConnection connection = new SQLiteConnection(_conString))
            {
                SQLiteCommand command;
                connection.Open();
                foreach (var table in sqlString)
                {
                    command = new SQLiteCommand(table, connection);
                    command.ExecuteNonQuery();
                }
            }
            DummyData.FillDatabase();
        }
    }
}