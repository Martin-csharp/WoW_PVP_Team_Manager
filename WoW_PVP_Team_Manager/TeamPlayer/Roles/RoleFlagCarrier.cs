using System;
using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.TeamPlayer.Roles
{
    class RoleFlagCarrier : Player, IPlayerRole
    {
        //Creates object of player from database
        public RoleFlagCarrier(PlayerModel model) : base(model) { }


        public override void PrimaryAbility()
        {
            Custom.Message("I can take a lot of damage and survive.");
        }

        public override void SecondaryAbility()
        {
            Custom.Message("If needed I'll try and do as much damage as i can to the enemy players.");
        }

        public override void FocusTarget()
        {
            Custom.Message("My main focus is getting the enemy flag and returning it to our camp.");
        }
    }
}
