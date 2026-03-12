using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague
{
    /// <summary>
    /// Основной менеджер футбольной лиги
    /// </summary>
    public class LeagueManager
    {
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<Match> Matches { get; set; } = new List<Match>();
        public List<TeamRoundStats> RoundHistory { get; set; } = new List<TeamRoundStats>();
        private int lastRecordedRound = 0;

        /// <summary>
        /// Обрабатывает результат матча и обновляет статистику команд
        /// </summary>
        /// <param name="match">Матч для обработки</param>
        public void ProcessMatchResult(Match match)
        {
            if (!match.IsPlayed) 
            {
                Console.WriteLine($"Матч {match.Id} еще не сыгран");
                return;
            }

            Console.WriteLine($"Обработка матча: {match.HomeTeam.Name} {match.HomeGoals}:{match.AwayGoals} {match.AwayTeam.Name}");

            match.HomeTeam.Played++;
            match.AwayTeam.Played++;

            match.HomeTeam.GoalsFor += match.HomeGoals;
            match.HomeTeam.GoalsAgainst += match.AwayGoals;

            match.AwayTeam.GoalsFor += match.AwayGoals;
            match.AwayTeam.GoalsAgainst += match.HomeGoals;

            if (match.HomeGoals > match.AwayGoals)
            {
                match.HomeTeam.Wins++;
                match.AwayTeam.Losses++;
                Console.WriteLine($"Победа хозяев: {match.HomeTeam.Name}");
            }
            else if (match.HomeGoals < match.AwayGoals)
            {
                match.AwayTeam.Wins++;
                match.HomeTeam.Losses++;
                Console.WriteLine($"Победа гостей: {match.AwayTeam.Name}");
            }
            else
            {
                match.HomeTeam.Draws++;
                match.AwayTeam.Draws++;
                Console.WriteLine("Ничья");
            }

            if (match.Round != lastRecordedRound)
            {
                RecordRoundStats(match.Round);
                lastRecordedRound = match.Round;
            }
        }

        private void RecordRoundStats(int round)
        {
            var standings = GetStandings();
            for (int i = 0; i < standings.Count; i++)
            {
                RoundHistory.Add(new TeamRoundStats
                {
                    TeamId = standings[i].Id,
                    TeamName = standings[i].Name,
                    Round = round,
                    Points = standings[i].Points,
                    Position = i + 1
                });
            }
        }

        public List<Team> GetStandings()
        {
            return Teams
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();
        }

        public List<Match> GetHeadToHead(Team team1, Team team2)
        {
            return Matches.Where(m =>
                (m.HomeTeam == team1 && m.AwayTeam == team2) ||
                (m.HomeTeam == team2 && m.AwayTeam == team1)
            ).ToList();
        }

        public List<Team> SearchTeams(Func<Team, bool> predicate)
        {
            return Teams.Where(predicate).ToList();
        }

        public List<TeamRoundStats> GetTeamHistory(int teamId)
        {
            return RoundHistory.Where(r => r.TeamId == teamId).OrderBy(r => r.Round).ToList();
        }
    }
}
