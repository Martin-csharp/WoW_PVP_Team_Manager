using System;
using System.Collections.Generic;
using System.Linq;
using WoW_PVP_Team_Manager.DataBase;
using WoW_PVP_Team_Manager.Models;
using WoW_PVP_Team_Manager.TeamPlayer;
using WoW_PVP_Team_Manager.TeamPlayer.Roles;


namespace WoW_PVP_Team_Manager.Controllers
{
    class PlayerController
    {        
        //Add Player
        public static void AddPlayer(PlayerModel model)
        {
            string sqlString = 
                "INSERT INTO Players (Team, Name, Date, Rank, Active, Role) " +
                "VALUES (@Team, @Name, @Date, @Rank, @Active, @Role);";
            DataAccess.SaveData(sqlString, model);
        }
        
        //Remove player 
        public static void DeletePlayer(Player p)
        {
            var model = PlayerToModel(p);
            string sqlString = "DELETE FROM Players WHERE Id = @Id";
            DataAccess.SaveData(sqlString, model);
        }

        //Change player role
        public static void EditRole(Player p, string role)
        {
            var model = PlayerToModel(p);
            model.Role = role;
            string sqlString = "UPDATE Players SET Role = @Role WHERE Id = @Id;";
            DataAccess.SaveData(sqlString, model);
        }

        //Change player to Active or InActive
        public static void EditActive(Player p)
        {
            var model = PlayerToModel(p);
            if (model.Active) model.Active = false;
            else if (!model.Active) model.Active = true;
            string sqlString = "UPDATE Players SET Active = @Active WHERE Id = @Id;";
            DataAccess.SaveData(sqlString, model);
        }

        //After a victory both teams and players that played in a tournament will gain a rank
        public static void UpdateRanks(int teamId)
        {
            var modelList        = new List<PlayerModel>();            
            var team             = App.Lists.TeamList.OfType<Team>().FirstOrDefault(x => x.Id == teamId); 
            var activePlayerList = team.PlayerList.OfType<Player>().Where(x => x.IsPlayingTournament).ToList(); 

            foreach (var player in activePlayerList)
            {
                modelList.Add(PlayerToModel(player));
            }

            string sqlString = "UPDATE Players SET Rank = @Rank WHERE Id = @Id;";
            DataAccess.SaveData(sqlString, modelList);
            sqlString = "UPDATE Teams SET Rank = @Rank WHERE Id = @Id;";
            DataAccess.SaveData(sqlString, team);

        }

        //Construct a player object (Polymorphism)
        public static Player CreatePlayer(PlayerModel model)
        {
            Player p;
            switch (Enum.Parse(typeof(Custom.Role), model.Role))
            {
                case Custom.Role.DPS:
                    p = new RoleDPS(model);
                    break;
                case Custom.Role.Healer:
                    p = new RoleHealer(model);
                    break;
                case Custom.Role.FlagCarrier:
                    p = new RoleFlagCarrier(model);
                    break;
                case Custom.Role.TeamCaptain:
                    p = new RoleTeamCaptain(model);
                    break;
                default: return null;
            }
            return p;
        }

        //Updates the player list with the latest data
        public static List<Player> UpdatePlayerList(Team team)
        {
            List<Player> list = new List<Player>();
            object parameter = new { TeamName = team.Name };
            string sqlString = "SELECT * FROM Players WHERE Team = @TeamName;";
            var model = DataAccess.LoadData<PlayerModel>(sqlString, parameter);

            foreach (var item in model)
            {
                list.Add(CreatePlayer(item));
            }
            return list;
        }

        //Player to model
        private static PlayerModel PlayerToModel(Player player)
        {
            return new PlayerModel()
            {
                Name   = player.Name,
                Date   = player.Date.ToString(),
                Role   = Enum.GetName(typeof(Custom.Role), player.Role),
                Active = player.Active,
                Id     = player.Id,
                Rank   = player.Rank,
                Team   = player.TeamName
            };
        }
    }
}
