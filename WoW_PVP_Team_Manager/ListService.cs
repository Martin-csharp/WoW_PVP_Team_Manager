using System.Collections.ObjectModel;
using WoW_PVP_Team_Manager.Models;
using WoW_PVP_Team_Manager.TeamPlayer;

namespace WoW_PVP_Team_Manager
{
    public class ListService 
    {
        //Teams
        private ObservableCollection<Team> _teamList = new ObservableCollection<Team>();
        internal ObservableCollection<Team> TeamList { get => _teamList; }

        internal void AddTeam(TeamModel model)
        {
            _teamList.Add(new Team(model.Id, model.Name, model.Rank));
        }

        //Logs
        private ObservableCollection<TournamentLogs> _logList = new ObservableCollection<TournamentLogs>();
        internal ObservableCollection<TournamentLogs> LogList { get { return _logList; } }

       internal void AddLog(LogListModel model)
        {
            _logList.Add(new TournamentLogs(model));            
        } 
    }
}
