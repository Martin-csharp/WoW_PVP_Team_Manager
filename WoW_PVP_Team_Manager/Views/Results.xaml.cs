using Dapper;
using System.Data.SqlTypes;
using System.Linq;
using Windows.UI.Xaml.Controls;
using WoW_PVP_Team_Manager.DataBase;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.Views
{
    public sealed partial class Results : Page
    {
        public Results()
        {
            InitializeComponent();
            ListBox_LogList.ItemsSource = App.Lists.LogList;
        }

        //Update Listboxes with selected log data
        private void ListBox_LogList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox_LogList.SelectedIndex == -1) return;
            var list = App.Lists.LogList[ListBox_LogList.SelectedIndex];
            var a = list.LogsTeamA();
            var b = list.LogsTeamB();
            ListBox_TeamA.Items.Clear();
            ListBox_TeamB.Items.Clear();
            Txt_TeamA.Text = a.First().TeamName;
            Txt_TeamB.Text = b.First().TeamName;

            foreach (var item in a)
            {
                ListBox_TeamA.Items.Add($"{item.PlayerName}, {item.Role}, {item.Description} at minute {item.TimeStamp}");
            }

            foreach (var item in b)
            {
                ListBox_TeamB.Items.Add($"{item.PlayerName}, {item.Role}, {item.Description} at minute {item.TimeStamp}");
            }
        }

        //Delete all logs
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string sqlString = "DELETE FROM LogLists";
            DataAccess.SaveData(sqlString, new DynamicParameters());
            sqlString = "DELETE FROM LogItems";
            DataAccess.SaveData(sqlString, new DynamicParameters());
            App.Lists.LogList.Clear();
            ListBox_TeamA.Items.Clear();
            ListBox_TeamB.Items.Clear();
        }
    }
}
