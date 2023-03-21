namespace WoW_PVP_Team_Manager.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }

        public TeamModel() { }

        //Constructor to handle dummy data
        public TeamModel(string name)
        {
            Name = name;
            Rank = 0;
        }
    }
}
