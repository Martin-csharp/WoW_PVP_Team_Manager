using System;
using WoW_PVP_Team_Manager.Controllers;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.TeamPlayer
{
    class Player
    {
        //'private set' forces the use of constructors needed for polymorphism.
        public string Name              { get; private set; }
        public string TeamName          { get; private set; }
        public int Id                   { get; private set; }
        public int Rank                 { get; private set; }
        public bool Active              { get; private set; }
        public Custom.Role Role         { get; private set; }
        public DateTime Date            { get; private set; }
        public bool IsPlayingTournament { get; private set; }

        //Adding player from database
        public Player(PlayerModel p)
        {
            Id       = p.Id;
            Name     = p.Name;
            Date     = DateTime.Parse(p.Date);
            Rank     = p.Rank;
            Active   = p.Active;
            Role     = (Custom.Role)Enum.Parse(typeof(Custom.Role), p.Role);
            TeamName = p.Team;
        }   
        
        //Player is in a tournament (and cannot be edited until after)
        public void StartTournament() => IsPlayingTournament = true;
        public void EndTournament()   => IsPlayingTournament = false;

        //Victory!
        public void Victory(int teamId)
        {
            if (IsPlayingTournament)
            {
                Rank++;
                PlayerController.UpdateRanks(teamId);
            }
        }

        //Retrieve player age from date 
        public int GetAge()  => Convert.ToInt16((DateTime.Today.Year - Date.Year));

        //Role specific functions that need to be overwritten when creating object of class
        public virtual void PrimaryAbility()   { }
        public virtual void SecondaryAbility() { }
        public virtual void FocusTarget()      { }
    }
}