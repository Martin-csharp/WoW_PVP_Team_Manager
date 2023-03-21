using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.UI.Xaml.Controls;
using WoW_PVP_Team_Manager.DataBase;
using WoW_PVP_Team_Manager.Models;
using WoW_PVP_Team_Manager.TeamPlayer;

namespace WoW_PVP_Team_Manager.Views
{
    public sealed partial class Tournament : Page
    {
        private List<LogModel> _logList;

        public Tournament()
        {
            InitializeComponent();
            _logList = new List<LogModel>();
            Btn_PlayTournament.IsEnabled = false;
            ListBox_PlayerA.ItemsSource = App.Lists.TeamList;
            ListBox_PlayerB.ItemsSource = App.Lists.TeamList;
        }       

        private async void PlayTournament(Team a, Team b)
        {
            //Check if teams are eligible to play tournament
            if (!Validate()) return;

            int time = 0;
            int randomSelect;
            int teamTurn;
            int scoreA = 0;
            int scoreB = 0;
            string name;
            string teamName;
            string description = null;
            string winner = null;
            Custom.Role role;
            Team currentTeam;
            Player selectedPlayer;
            Random random = new Random();

            _logList.Clear();
            ListView_A.Items.Clear();
            ListView_B.Items.Clear();
            Btn_PlayTournament.IsEnabled = false;
            ListBox_PlayerA.IsEnabled = false;
            ListBox_PlayerB.IsEnabled = false;
            ListBox_PlayerA.SelectedIndex = -1;
            ListBox_PlayerB.SelectedIndex = -1;
            a.PlayerList.Where(x => x.Active).ToList().ForEach(x => x.StartTournament());
            b.PlayerList.Where(x => x.Active).ToList().ForEach(x => x.StartTournament());

            while (time != 20)
            {
                //Simulate tournament taking time
                await Task.Delay(200);
                time++;

                //Deceide team turn
                teamTurn = random.Next(-1, 1);
                if (teamTurn == 0) currentTeam = a;
                else               currentTeam = b;

                //Deceide what player will do something
                do
                {                    
                    randomSelect   = random.Next(0, currentTeam.GetPlayerCount() - 1); //Select a random player 
                    role           = currentTeam.PlayerList[randomSelect].Role;        //Get player role
                    name           = currentTeam.PlayerList[randomSelect].Name;        //Get player name
                    teamName       = currentTeam.Name;                                 //Get current team
                    selectedPlayer = currentTeam.PlayerList[randomSelect];
                } while (selectedPlayer.Role == Custom.Role.TeamCaptain || !selectedPlayer.Active);

                //Player action
                switch (role)
                { 
                    case Custom.Role.DPS:
                        description = "retrieved their flag"; 
                        break;
                    case Custom.Role.Healer:
                        description = "did a great heal";
                        break;
                    case Custom.Role.FlagCarrier:
                        description = "captured the enemy flag";
                        if (currentTeam == a) scoreA++;
                        else scoreB++;
                        break;
                }

                //Add action and player to log
                string teamId = null;
                if (currentTeam == a)      teamId = "a";
                else if (currentTeam == b) teamId = "b";
                var newLog = new LogModel()
                {
                    TeamId      = teamId,
                    TeamName    = teamName,
                    PlayerName  = name,
                    TimeStamp   = time,
                    Role        = Enum.GetName(typeof(Custom.Role), role),
                    Description = description
                };
                Txt_ScoreA.Text = scoreA.ToString();
                Txt_ScoreB.Text = scoreB.ToString();

                //Shows latest log either listview a or b
                UpdatLogList(newLog);
            }

            //Print scores to textblock and show match results etc.
            if (scoreA > scoreB)
            {
                a.Victory();
                winner = a.Name;
                Custom.Message($"Team {a.Name} has won with a score of {scoreA}");
            }
            else if (scoreB > scoreA)
            {
                b.Victory();
                winner = b.Name;
                Custom.Message($"Team {b.Name} has won with a score of {scoreB}");
            }
            else if (scoreA == scoreB)
            {
                winner = "Draw, no";
                Custom.Message("Its a draw");
            }

            ListBox_PlayerA.IsEnabled = true;
            ListBox_PlayerB.IsEnabled = true;   
            a.PlayerList.Where(x => x.IsPlayingTournament).ToList().ForEach(x => x.EndTournament());
            b.PlayerList.Where(x => x.IsPlayingTournament).ToList().ForEach(x => x.EndTournament());
            
            //Save all the logs
            SaveLogList(winner);
        }

        //Save log list
        private void SaveLogList(string winner)
        {
            LogListModel model = new LogListModel();
            var sqlString = "INSERT INTO LogLists (Title) VALUES (@Title);";
            var timeStamp = DateTime.Now.ToString("dd/MM/yy HH:mm");
            var teamA = _logList.Where(x => x.TeamId == "a").First().TeamName;
            var teamB = _logList.Where(x => x.TeamId == "b").First().TeamName;
            model.Title = $"{timeStamp} : {teamA} vs {teamB} : {winner} victory";
            DataAccess.SaveData(sqlString, model);

            //Save log items
            if (App.Lists.LogList.Count == 0)
            {
                foreach (var item in _logList)
                {
                    item.TournamentId = 1; 
                }
                model.Id = 1;
            }
            else
            {
                foreach (var item in _logList)
                {
                    item.TournamentId = App.Lists.LogList.Last().Id + 1;
                }
                model.Id = App.Lists.LogList.Last().Id +1;
            }
            sqlString = "INSERT INTO LogItems (TournamentId, TimeStamp, TeamId, TeamName, PlayerName, Description, Role) VALUES (@TournamentId, @TimeStamp, @TeamId, @TeamName, @PlayerName, @Description, @Role);";
            DataAccess.SaveData(sqlString, _logList);

            //Save to memory
            App.Lists.AddLog(model);
        }

        //Update the log with lates updates
        private void UpdatLogList(LogModel log)
        {
            if (log.TeamId == "a") ListView_A.Items.Add($"{log.PlayerName} from team {log.TeamName} {log.Description}.");   
            else ListView_B.Items.Add($"{log.PlayerName} from team {log.TeamName} {log.Description}.");
            _logList.Add(log);
        }

        //Button to play tournament
        private void Btn_PlayTournament_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var a = App.Lists.TeamList[ListBox_PlayerA.SelectedIndex];
            var b = App.Lists.TeamList[ListBox_PlayerB.SelectedIndex];
            PlayTournament(a, b);
        }

        //Check for unique selection
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox_PlayerA.SelectedItem == ListBox_PlayerB.SelectedItem)
            {
                if (ListBox_PlayerA.SelectedItem == null) return;
                Custom.Message("Please select 2 unique teams to compete against each other.");
                Btn_PlayTournament.IsEnabled = false;
                return;
            }

            if (ListBox_PlayerA.SelectedItem != null && ListBox_PlayerB.SelectedItem != null) Btn_PlayTournament.IsEnabled = true;
            else Btn_PlayTournament.IsEnabled = false;
        }

        //Check if team has enough active players and an active team captain
        private bool Validate()
        {
            int indexA = ListBox_PlayerA.SelectedIndex;
            int indexB = ListBox_PlayerB.SelectedIndex;
            var a = App.Lists.TeamList[indexA];
            var b = App.Lists.TeamList[indexB];
            if (a.GetActivePlayers() != 10 || b.GetActivePlayers() != 10)
            {
                Custom.Message("Teams need 10 active players to play in a tournament!");
                return false;
            }

            if (!a.PlayerList.Any(x => x.Role == Custom.Role.TeamCaptain && x.Active))
            {
                Custom.Message("Teams need an active TeamCaptain to play in a tournament!");
                return false;
            }
            return true;
        }
    }
}
