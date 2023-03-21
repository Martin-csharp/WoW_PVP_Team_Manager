namespace WoW_PVP_Team_Manager.TeamPlayer.Roles
{
    interface IPlayerRole
    {
        //A player must have these abilities
        void PrimaryAbility();
        void SecondaryAbility();
        void FocusTarget();
    }
}
