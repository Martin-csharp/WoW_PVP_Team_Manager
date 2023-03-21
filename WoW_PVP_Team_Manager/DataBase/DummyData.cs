using System;
using System.Collections.Generic;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.DataBase
{
    public static class DummyData
    {
        private static List<TeamModel> _teams = new List<TeamModel>();
        private static List<PlayerModel> _players = new List<PlayerModel>();

        private static void GetDummyData()
        {
            _teams.Add(new TeamModel("Alpha"));
            _teams.Add(new TeamModel("Beta"));
            _teams.Add(new TeamModel("Charlie"));
            _teams.Add(new TeamModel("Delta"));
            _teams.Add(new TeamModel("Echo"));

            _players.Add(new PlayerModel("Alpha", "Arty", DateTime.Now.AddYears(-25), Custom.Role.TeamCaptain));
            _players.Add(new PlayerModel("Alpha", "Aiden", DateTime.Now.AddYears(-22), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Alpha", "Abigail", DateTime.Now.AddYears(-29), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Alpha", "Andrew", DateTime.Now.AddYears(-27), Custom.Role.Healer));
            _players.Add(new PlayerModel("Alpha", "Aaron", DateTime.Now.AddYears(-20), Custom.Role.Healer));
            _players.Add(new PlayerModel("Alpha", "Austin", DateTime.Now.AddYears(-31), Custom.Role.DPS));
            _players.Add(new PlayerModel("Alpha", "Axel", DateTime.Now.AddYears(-30), Custom.Role.DPS));
            _players.Add(new PlayerModel("Alpha", "Adam", DateTime.Now.AddYears(-20), Custom.Role.DPS));
            _players.Add(new PlayerModel("Alpha", "Ava", DateTime.Now.AddYears(-25), Custom.Role.DPS));
            _players.Add(new PlayerModel("Alpha", "Alex", DateTime.Now.AddYears(-27), Custom.Role.DPS));

            _players.Add(new PlayerModel("Beta", "Bruce", DateTime.Now.AddYears(-25), Custom.Role.TeamCaptain));
            _players.Add(new PlayerModel("Beta", "Bert", DateTime.Now.AddYears(-22), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Beta", "Ben", DateTime.Now.AddYears(-29), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Beta", "Bonnie", DateTime.Now.AddYears(-27), Custom.Role.Healer));
            _players.Add(new PlayerModel("Beta", "Braelyn", DateTime.Now.AddYears(-20), Custom.Role.Healer));
            _players.Add(new PlayerModel("Beta", "Bruno", DateTime.Now.AddYears(-31), Custom.Role.DPS));
            _players.Add(new PlayerModel("Beta", "Bryce", DateTime.Now.AddYears(-30), Custom.Role.DPS));
            _players.Add(new PlayerModel("Beta", "Bjorn", DateTime.Now.AddYears(-20), Custom.Role.DPS));
            _players.Add(new PlayerModel("Beta", "Bernard", DateTime.Now.AddYears(-25), Custom.Role.DPS));
            _players.Add(new PlayerModel("Beta", "Bree", DateTime.Now.AddYears(-27), Custom.Role.DPS));

            _players.Add(new PlayerModel("Charlie", "Charles", DateTime.Now.AddYears(-25), Custom.Role.TeamCaptain));
            _players.Add(new PlayerModel("Charlie", "Chris", DateTime.Now.AddYears(-22), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Charlie", "Cooper", DateTime.Now.AddYears(-29), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Charlie", "Cole", DateTime.Now.AddYears(-27), Custom.Role.Healer));
            _players.Add(new PlayerModel("Charlie", "Chester", DateTime.Now.AddYears(-20), Custom.Role.Healer));
            _players.Add(new PlayerModel("Charlie", "Carlos", DateTime.Now.AddYears(-31), Custom.Role.DPS));
            _players.Add(new PlayerModel("Charlie", "Charlotte", DateTime.Now.AddYears(-30), Custom.Role.DPS));
            _players.Add(new PlayerModel("Charlie", "Cora", DateTime.Now.AddYears(-20), Custom.Role.DPS));
            _players.Add(new PlayerModel("Charlie", "Cody", DateTime.Now.AddYears(-25), Custom.Role.DPS));
            _players.Add(new PlayerModel("Charlie", "Corwin", DateTime.Now.AddYears(-27), Custom.Role.DPS));

            _players.Add(new PlayerModel("Delta", "Dave", DateTime.Now.AddYears(-30), Custom.Role.TeamCaptain));
            _players.Add(new PlayerModel("Delta", "Dennis", DateTime.Now.AddYears(-26), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Delta", "Dora", DateTime.Now.AddYears(-19), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Delta", "Darion", DateTime.Now.AddYears(-21), Custom.Role.Healer));
            _players.Add(new PlayerModel("Delta", "Denise", DateTime.Now.AddYears(-31), Custom.Role.Healer));
            _players.Add(new PlayerModel("Delta", "David", DateTime.Now.AddYears(-24), Custom.Role.DPS));
            _players.Add(new PlayerModel("Delta", "Daniel", DateTime.Now.AddYears(-26), Custom.Role.DPS));
            _players.Add(new PlayerModel("Delta", "Diego", DateTime.Now.AddYears(-29), Custom.Role.DPS));
            _players.Add(new PlayerModel("Delta", "Dylan", DateTime.Now.AddYears(-25), Custom.Role.DPS));
            _players.Add(new PlayerModel("Delta", "Delilah", DateTime.Now.AddYears(-27), Custom.Role.DPS));

            _players.Add(new PlayerModel("Echo", "Erik", DateTime.Now.AddYears(-25), Custom.Role.TeamCaptain));
            _players.Add(new PlayerModel("Echo", "Ethan", DateTime.Now.AddYears(-26), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Echo", "Ezra", DateTime.Now.AddYears(-29), Custom.Role.FlagCarrier));
            _players.Add(new PlayerModel("Echo", "Evelyn", DateTime.Now.AddYears(-21), Custom.Role.Healer));
            _players.Add(new PlayerModel("Echo", "Elizabeth", DateTime.Now.AddYears(-28), Custom.Role.Healer));
            _players.Add(new PlayerModel("Echo", "Ezekiel", DateTime.Now.AddYears(-22), Custom.Role.DPS));
            _players.Add(new PlayerModel("Echo", "Elliot", DateTime.Now.AddYears(-27), Custom.Role.DPS));
            _players.Add(new PlayerModel("Echo", "Evan", DateTime.Now.AddYears(-29), Custom.Role.DPS));
            _players.Add(new PlayerModel("Echo", "Edgar", DateTime.Now.AddYears(-24), Custom.Role.DPS));
            _players.Add(new PlayerModel("Echo", "Enrique", DateTime.Now.AddYears(-22), Custom.Role.DPS));
        }

        public static void FillDatabase()
        {
            string teamString = "INSERT INTO Teams (Name, Rank) VALUES (@Name, @Rank);";
            string playerString = "INSERT INTO Players (Team, Name, Date, Rank, Active, Role) VALUES (@Team, @Name, @Date, @Rank, @Active, @Role);";

            GetDummyData();

            DataAccess.SaveData(teamString, _teams);
            DataAccess.SaveData(playerString, _players);
        }
    }
}
