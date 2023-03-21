using WoW_PVP_Team_Manager.Models;

namespace WoW_PVP_Team_Manager.TeamPlayer.Roles
{
    class RoleHealer : Player, IPlayerRole
    {
        //Creates object of player from database
        public RoleHealer(PlayerModel model) : base(model) { }

        public override void PrimaryAbility()
        {
            Custom.Message("I try to heal wounded teammates whenever i can.");
        }

        public override void SecondaryAbility()
        {
            Custom.Message("If needed i'll try to do as much damage as i can to the enemy players.");
        }

        public override void FocusTarget()
        {
            Custom.Message("My main focus is to keep our flag carrier alive.");
        }
    }
}
