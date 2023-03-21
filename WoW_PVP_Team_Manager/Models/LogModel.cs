namespace WoW_PVP_Team_Manager.Models
{
    public class LogModel
    {
        public int Id             { get; set; }
        public int TournamentId   { get; set; }
        public int TimeStamp      { get; set; }
        public string TeamId      { get; set; }
        public string TeamName    { get; set; }
        public string PlayerName  { get; set; }
        public string Description { get; set; }
        public string Role        { get; set; }        
    }
}
