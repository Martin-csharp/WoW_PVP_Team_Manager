using System;
using System.Collections.Generic;
using System.Linq;
using WoW_PVP_Team_Manager.DataBase;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager
{
    class TournamentLogs
    {
        public int Id { get; set; }
        public string Title { get; private set; }

        private List<LogModel> _logItems;

        public TournamentLogs(LogListModel model) 
        {
            Id = model.Id;
            Title = model.Title;
            LoadLogs();
        }

        private void LoadLogs()
        {
            object parameters = new { TournamentId = Id };
            string sqlString = "SELECT * FROM LogItems WHERE TournamentId = @TournamentId;";
            _logItems = DataAccess.LoadData<LogModel>(sqlString, parameters);
            _logItems = _logItems.OrderBy(x => x.TimeStamp).ToList();
        }

        public List<LogModel> LogsTeamA() => _logItems.Where(x => x.TeamId == "a").ToList();
        public List<LogModel> LogsTeamB() => _logItems.Where(x => x.TeamId == "b").ToList();
    }
}
