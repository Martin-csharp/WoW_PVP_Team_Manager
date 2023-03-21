using System;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.TeamPlayer.Roles
{
    class RoleDPS : Player, IPlayerRole
    {
        //Creates object of player from database
        public RoleDPS(PlayerModel model) : base(model) { }

        public override void PrimaryAbility()
        {
            Custom.Message("I'm dealing as much damage to other players as i can.");
        }

        public override void SecondaryAbility()
        {
            Custom.Message("I'll taunt enemy players to keep them away from healers and flag carriers.");
        }

        public override void FocusTarget()
        {
            Custom.Message("My main focus are the enemy flag carriers.");
        }
    }
}
