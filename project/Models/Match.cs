namespace FootballLeague
{
    public class Match
    {
        public int Id { get; set; }
        public int Round { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public bool IsPlayed { get; set; }
    }
}
