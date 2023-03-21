using System;

namespace WoW_PVP_Team_Manager.Models
{
    public class PlayerModel
    {
        public int Id      { get; set; }
        public string Team { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public int Rank    { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }

        public PlayerModel() { }

        //Constructor to handle dummy data
        public PlayerModel(string team, string name, DateTime date, Custom.Role role)
        {
            Team   = team;
            Name   = name;
            Date   = date.ToString();
            Rank   = 0;
            Active = true;
            Role   = role.ToString();
        }
    }
}