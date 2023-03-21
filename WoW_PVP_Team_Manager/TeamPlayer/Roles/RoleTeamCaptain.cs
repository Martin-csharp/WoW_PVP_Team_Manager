using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.TeamPlayer.Roles
{
    class RoleTeamCaptain : Player, IPlayerRole
    {
        //Creates object of player from database
        public RoleTeamCaptain(PlayerModel model) : base(model) { }

        public override void PrimaryAbility()
        {
            Custom.Message("I'll lead our team to victory.");
        }

        public override void SecondaryAbility()
        {
            Custom.Message("I'll adjust strategies when needed.");
        }

        public override void FocusTarget()
        { 
            Custom.Message("My main focus is the entiere team.");
        }
    }
}
