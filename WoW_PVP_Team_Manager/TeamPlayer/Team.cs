using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WoW_PVP_Team_Manager.Controllers;

namespace WoW_PVP_Team_Manager.TeamPlayer
{
    class Team
    {
        private List<Player> _playerList = new List<Player>();
        public ObservableCollection<Player> PlayerList => new ObservableCollection<Player>(_playerList);

        public int Id      { get; private set; }
        public int Rank    { get; private set; }
        public string Name { get; private set; }        
               

        //Constructor is used when team is built from the database
        public Team(int id, string name, int teamRank)
        {
            Id          = id;
            Name        = name;
            Rank        = teamRank;
            _playerList = GetNewList();
        }

        //Retrieving team info                            
        public int GetPlayerCount()         => _playerList.Count;
        public int GetActivePlayers()       => _playerList.Count(player => player.Active);
        public int GetTotalDPS()            => _playerList.Count(player => player.Role == Custom.Role.DPS);
        public int GetTotalFlagCarriers()   => _playerList.Count(player => player.Role == Custom.Role.FlagCarrier);
        public int GetTotalHealers()        => _playerList.Count(player => player.Role == Custom.Role.Healer);
        public int GetTotalTeamCaptains()   => _playerList.Count(player => player.Role == Custom.Role.TeamCaptain);

        //Rank up after victory
        public void Victory() 
        {
            Rank++;
            TeamController.UpdateRank(Id);
            PlayerList.ToList().ForEach(x => x.Victory(Id));           
        }
        
        //Reload the player list with latest edits
        public void UpdatePlayerList() => _playerList = GetNewList();

        //Get a new player list from the database
        private List<Player> GetNewList() => PlayerController.UpdatePlayerList(this).OrderByDescending(x => x.Name).ToList();
    }
}
