using System;
using Windows.UI.Popups;

namespace WoW_PVP_Team_Manager
{
    public static class Custom
    {
        //Forcing the use of the correct roles throughout all the classes
        public enum Role
        {
            DPS,
            Healer,
            FlagCarrier,
            TeamCaptain
        }

        //Making messages easier to call
        public static async void Message(string message) => await new MessageDialog(message).ShowAsync();
    }
}

