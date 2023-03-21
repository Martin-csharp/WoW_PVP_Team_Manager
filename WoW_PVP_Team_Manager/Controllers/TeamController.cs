using Dapper;
using System.Collections.Generic;
using System.Linq;
using WoW_PVP_Team_Manager.DataBase;
using WoW_PVP_Team_Manager.Models;
using WoW_PVP_Team_Manager.TeamPlayer;

namespace WoW_PVP_Team_Manager.Controllers
{
    static class TeamController
    {
        //Add team
        static public void AddTeam(string teamName)
        {
            string sqlString = "INSERT INTO Teams (Name, Rank) VALUES (@Name, @Rank)";
            DataAccess.SaveData(sqlString, new TeamModel(teamName));
            Custom.Message($"Succesfully added {teamName} to the Team List.");
        }

        //Remove team
        static public void DeleteTeam(Team team)
        {
            object parameter = new { Id = team.Id };
            string sqlString = "DELETE FROM Teams WHERE Id = @Id";
            DataAccess.SaveData(sqlString, parameter);
        }

        //Update team rank
        static public void UpdateRank(int teamId)
        {
            var team = App.Lists.TeamList.OfType<Team>().FirstOrDefault(x => x.Id == teamId);
            TeamModel model = new TeamModel() 
            {
                Id = team.Id,
                Rank = team.Rank
            };
            string sqlString = "UPDATE Teams SET Rank = @Rank WHERE Id = @Id;";
            DataAccess.SaveData(sqlString, model);
        }

        //Updates the team list
        public static void UpdateTeamList()
        {
            var data = DataAccess.LoadData<TeamModel>("SELECT * FROM Teams", new DynamicParameters());
            App.Lists.TeamList.Clear();

            foreach (var item in data)
            {
                App.Lists.AddTeam(item);
            }
        }
    }
}
