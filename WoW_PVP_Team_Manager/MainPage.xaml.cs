using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using WoW_PVP_Team_Manager.DataBase;
using System.Threading.Tasks;
using Dapper;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager
{    
    public partial class MainPage : Page
    {       
        //Default constructor
        public MainPage()
        {
            InitializeComponent();
            Task.Run(async () => await DataAccess.Initialize()).Wait();
            ApplicationView.PreferredLaunchViewSize      = new Size(1280, 730);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ContentFrame.Navigate(typeof(Views.TeamManager));
            NavigationPanel.SelectedItem = Nav_TeamManager;
            InitializeUI();
        }

        private void InitializeUI()
        {
            var logs = DataAccess.LoadData<LogListModel>("SELECT * FROM LogLists", new DynamicParameters());
            foreach (var item in logs)
            {
                App.Lists.AddLog(item);
            }

            var teams = DataAccess.LoadData<TeamModel>("SELECT * FROM Teams", new DynamicParameters());
            App.Lists.TeamList.Clear();
            foreach (var item in teams)
            {
                App.Lists.AddTeam(item);
            }
        }

        private void NavigationPanel_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
            switch (item.Tag)
            {
                case "team":
                     ContentFrame.Navigate(typeof(Views.TeamManager));
                    break;
                case "tournament":
                    ContentFrame.Navigate(typeof(Views.Tournament));
                    break;
                case "results":
                    ContentFrame.Navigate(typeof(Views.Results));
                    break;
            }
        }
    }
}
