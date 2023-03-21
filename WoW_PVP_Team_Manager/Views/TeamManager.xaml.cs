using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WoW_PVP_Team_Manager.Models;
using WoW_PVP_Team_Manager.TeamPlayer;
using WoW_PVP_Team_Manager.Controllers;

namespace WoW_PVP_Team_Manager.Views
{
    public sealed partial class TeamManager : Page
    {
        //Easier readability with these references
        private int PlayerIndex()   => ListBox_Player.SelectedIndex;
        private int TeamIndex()     => ListBox_Team.SelectedIndex;
        private object PlayerItem() => ListBox_Player.SelectedItem;
        private object TeamItem()   => ListBox_Team.SelectedItem;
        private Team Team()         => App.Lists.TeamList[TeamIndex()];
        private Player Player()     => Team().PlayerList[PlayerIndex()];        

        //Default constructor
        public TeamManager()
        {
            InitializeComponent();            
            DatePlayer.MaxDate = DateTime.Now;
            ComboBoxPlayer_Role.ItemsSource = Enum.GetValues(typeof(Custom.Role));
            ListBox_Team.ItemsSource = App.Lists.TeamList;

            //Disable unusable elements 
            BtnActive.IsEnabled         = false;
            BtnDelete_Player.IsEnabled  = false;
            BtnDelete_Team.IsEnabled    = false;
            BtnPrimary.IsEnabled        = false;
            BtnSecondary.IsEnabled      = false;
            BtnFocus.IsEnabled          = false;
            Btn_AddPlayer.IsEnabled     = false;
        }

        //############################
        //######### Buttons ##########
        //############################

        //Button for adding a team to the list
        private void Button_AddTeam(object sender, RoutedEventArgs e)
        {
            string name = Txt_TeamName.Text;

            //Check for valid input
            if (name == "Enter team name..." || name == "")
            {
                Custom.Message("Please enter a valid name...");
                return;
            }

            //Check for duplicate team names
            if (App.Lists.TeamList.Any(x => x.Name.ToUpper() == name.ToUpper()))
            {
                Custom.Message($"The name \'{name}\' is already in use, please choose a different name.");
                return;
            }

            //Add team to list
            TeamController.AddTeam(name);

            //Update list with new data
            TeamController.UpdateTeamList();

            //Update UI
            Txt_TeamName.Text = "Enter new team name...";
        }

        //Button for deleting a team
        private void Button_DeleteTeam(object sender, RoutedEventArgs e)
        {
            if (!Team().PlayerList.Any(x => x.IsPlayingTournament))
            {
                TeamController.DeleteTeam(Team());

                //Update UI
                TeamController.UpdateTeamList();
                UpdatePlayerListBox();
                UpdatePlayerInfo();
            }
            else Custom.Message("This team is playing a tournament!");
        }

        //Button to add player to a team
        private void Button_AddPlayer(object sender, RoutedEventArgs e)
        {
            PlayerModel model = new PlayerModel();
            var radioButton = Group_Role.Children.OfType<RadioButton>().Where(r => r.IsChecked == true).FirstOrDefault();

            try
            {
                //Check for valid name 
                if (Txt_PlayerName.Text == "Enter player name..." || string.IsNullOrWhiteSpace(Txt_PlayerName.Text))
                    throw new Exception("Please enter a valid name!");

                //Check if date is selected
                if (DatePlayer == null)
                    throw new Exception("Please enter your date of birth!");

                //Check for valid age
                if ((DateTime.Now.Year - DatePlayer.Date.Value.Date.Year) <= 16)
                    throw new Exception("You need to be at least 16 years of age!");

                //Check for selected role
                if (radioButton == null)
                    throw new Exception("Please select a role!");

                //Set the approved values
                model.Name = Txt_PlayerName.Text;
                model.Date = DatePlayer.Date.Value.Date.ToString();
                model.Team = Team().Name;

                //Radiobutton selection
                switch (radioButton.Name)
                {                    
                    case "Rb_DPS":
                        model.Role = Enum.GetName(typeof(Custom.Role), Custom.Role.DPS);
                        break;
                    case "Rb_FlagCarrier":
                        model.Role = Enum.GetName(typeof(Custom.Role), Custom.Role.FlagCarrier);
                        break;
                    case "Rb_Healer":
                        model.Role = Enum.GetName(typeof(Custom.Role), Custom.Role.Healer);
                        break;
                    case "Rb_TeamCaptain":
                        model.Role = Enum.GetName(typeof(Custom.Role), Custom.Role.TeamCaptain);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                Custom.Message(ex.Message);
                return;
            }

            //Adding player to team
            PlayerController.AddPlayer(model);
            Custom.Message($"Player {model.Name} was succesfully added to team {Team().Name} as {model.Role}");

            //Get new data from database
            Team().UpdatePlayerList();

            //Update UI
            UpdatePlayerListBox();
            UpdateTeamInfo();
            Txt_PlayerName.Text = "Enter player name...";
            DatePlayer.Date = null;
            foreach (var rb in Group_Role.Children.OfType<RadioButton>())
            {
                rb.IsChecked = false;
            }
        }

        //Button to set new selected role for player
        private void Button_EditPlayer(object sender, RoutedEventArgs e)
        {
            //Check if player is busy
            if (!Player().IsPlayingTournament)
            {
                PlayerController.EditRole(Player(), ComboBoxPlayer_Role.SelectedItem.ToString());
                Team().UpdatePlayerList();

                //Update UI
                UpdateTeamInfo();
                UpdatePlayerInfo();
                ComboBoxPlayer_Role.SelectedIndex = -1;
            }
            else Custom.Message("This player is playing a tournament!");
        }

        //Button for deleting a player from a team
        private void Button_DeletePlayer(object sender, RoutedEventArgs e)
        {
            //Check if player is busy
            if (!Player().IsPlayingTournament)
            {
                PlayerController.DeletePlayer(Player());
                Team().UpdatePlayerList();

                //Update UI
                UpdatePlayerListBox();
                UpdateTeamInfo();
                UpdatePlayerInfo();
            }
            else Custom.Message("This player is playing a tournement!");
        }

        //Set player to inactive or inactive
        private void Button_IsActive_Click(object sender, RoutedEventArgs e)
        {
            //Check if player is busy
            if (!Player().IsPlayingTournament)
            {
                PlayerController.EditActive(Player());
                Team().UpdatePlayerList();

                //Update UI
                UpdateTeamInfo();
                UpdatePlayerInfo();
            }
            else Custom.Message("This player is playing a tournament!");
        }

        private void Button_Focus(object sender, RoutedEventArgs e)     => Player().FocusTarget();
        private void Button_Secondary(object sender, RoutedEventArgs e) => Player().SecondaryAbility();
        private void Button_Primary(object sender, RoutedEventArgs e)   => Player().PrimaryAbility();

        //############################
        //######## Functions #########
        //############################

        //Check if the selection has changed for the player listbox
        private void ListBox_Player_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlayerItem() == null)
            {
                BtnActive.IsEnabled = false;
                return;
            }
            BtnActive.IsEnabled = true;

            //Update UI
            UpdatePlayerInfo();
        }

        //Check if the selection has changed for the team listbox
        private void ListBox_Team_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Update the playerlistbox with the players for the currently selected team
            if (TeamItem() == null)
            {
                Btn_AddPlayer.IsEnabled = false;
                ListBox_Player.ItemsSource = null;                
                return;
            }
            Btn_AddPlayer.IsEnabled = true;

            //Update UI
            UpdatePlayerListBox();
            UpdatePlayerInfo();
            UpdateTeamInfo();
        }

        //Changes the player listbox's data source
        private void UpdatePlayerListBox()
        {
            if (TeamIndex() != -1)
            {
                ListBox_Player.ItemsSource = Team().PlayerList;
            }
            else ListBox_Player.ItemsSource = null;
        }

        //Get player info
        private void UpdatePlayerInfo()
        {
            if (PlayerItem() != null)
            {
                string buttonText;
                if (Player().Active) buttonText = "Active";
                else buttonText                 = "Inactive";

                Label_PlayerName.Text      = Player().Name;
                Label_PlayerAge.Text       = Player().GetAge().ToString();
                Label_PlayerRole.Text      = Player().Role.ToString();
                Label_PlayerRank.Text      = Player().Rank.ToString();
                BtnActive.IsChecked        = Player().Active;
                BtnDelete_Player.IsEnabled = true;
                BtnPrimary.IsEnabled       = true;
                BtnSecondary.IsEnabled     = true;
                BtnFocus.IsEnabled         = true;
                BtnActive.Content          = buttonText;
            }
            else
            {
                Label_PlayerName.Text      = "";
                Label_PlayerAge.Text       = "";
                Label_PlayerRole.Text      = "";
                Label_PlayerRank.Text      = "";
                BtnActive.IsEnabled        = false;
                BtnDelete_Player.IsEnabled = false;
                BtnPrimary.IsEnabled       = false;
                BtnSecondary.IsEnabled     = false;
                BtnFocus.IsEnabled         = false;
            }
        }

        //Get team info
        private void UpdateTeamInfo()
        {
            if (TeamItem() != null)
            {
                Label_TeamName.Text          = Team().Name;
                Label_TeamRank.Text          = Team().Rank.ToString();
                Label_TotalDPS.Text          = Team().GetTotalDPS().ToString();
                Label_TotalFlagCarriers.Text = Team().GetTotalFlagCarriers().ToString();
                Label_TotalHealers.Text      = Team().GetTotalHealers().ToString();
                Label_TotalTeamCaptains.Text = Team().GetTotalTeamCaptains().ToString();
                Label_TotalPlayers.Text      = Team().GetPlayerCount().ToString();
                Label_ActivePlayers.Text     = Team().GetActivePlayers().ToString() + "/10";
                BtnDelete_Team.IsEnabled     = true;
            }
            else
            {
                Label_TeamName.Text          = "";
                Label_TeamRank.Text          = "";
                Label_TotalDPS.Text          = "";
                Label_TotalFlagCarriers.Text = "";
                Label_TotalHealers.Text      = "";
                Label_TotalTeamCaptains.Text = "";
                Label_TotalPlayers.Text      = "";
                Label_ActivePlayers.Text     = "";
                BtnDelete_Team.IsEnabled     = false;
            }
        }

        //############################
        //## Clean UI functionality ##
        //############################

        //clears textbox when selected
        private void Txt_PlayerName_GotFocus(object sender, RoutedEventArgs e) => Txt_PlayerName.Text = "";  
        private void Txt_TeamName_GotFocus(object sender, RoutedEventArgs e)   => Txt_TeamName.Text = "";

        //When focus is lost the default text will reappear
        private void Txt_PlayerName_LostFocus(object sender, RoutedEventArgs e) => Txt(Txt_PlayerName, "Enter player name...");
        private void Txt_TeamName_LostFocus(object sender, RoutedEventArgs e)   => Txt(Txt_TeamName, "Enter team name...");

        private void Txt(TextBox textBox, string text)
        {
            if (textBox.Text == "") textBox.Text = text;
        }
    }
}

