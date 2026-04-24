using System;
using System.Collections.Generic;

namespace FootballLeague
{
    /// <summary>
    /// Main football league manager
    /// </summary>
    public partial class LeagueManager
    {
        public List<Team> Teams { get; set; } = new List<Team>();
        public List<Match> Matches { get; set; } = new List<Match>();
        public List<TeamRoundStats> RoundHistory { get; set; } = new List<TeamRoundStats>();

        /// <summary>
        /// Gets current standings with optimized sorting
        /// </summary>
        /// <returns>List of teams sorted by positions</returns>
        public List<Team> GetStandings()
        {
            // Filter only valid teams
            List<Team> validTeams = ManualAlgorithms.Where(Teams, t => t.IsValid());
            
            // Multi-level sorting
            List<Comparison<Team>> comparisons = new List<Comparison<Team>>
            {
                // By points (descending)
                (a, b) => b.Points.CompareTo(a.Points),
                // By goal difference (descending)
                (a, b) => b.GoalDifference.CompareTo(a.GoalDifference),
                // By goals for (descending)
                (a, b) => b.GoalsFor.CompareTo(a.GoalsFor),
                // By name (ascending)
                (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal)
            };
            
            return ManualAlgorithms.MultiSort(validTeams, comparisons);
        }

        /// <summary>
        /// Получает историю команды по турам
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns>История статистики команды</returns>
        public List<TeamRoundStats> GetTeamHistory(int teamId)
        {
            List<TeamRoundStats> filtered = ManualAlgorithms.Where(RoundHistory, r => r.TeamId == teamId);
            return ManualAlgorithms.OrderBy(filtered, r => r.Round);
        }

        /// <summary>
        /// Создает копию менеджера для прогнозирования результатов
        /// </summary>
        /// <returns>Копия менеджера с текущим состоянием</returns>
        public LeagueManager CreatePredictionCopy()
        {
            var copy = new LeagueManager();
            
            // Копируем команды
            var teamMapping = new Dictionary<int, Team>();
            for (int i = 0; i < Teams.Count; i++)
            {
                var team = Teams[i];
                var teamCopy = new Team
                {
                    Id = team.Id,
                    Name = team.Name,
                    Played = team.Played,
                    Wins = team.Wins,
                    Draws = team.Draws,
                    Losses = team.Losses,
                    GoalsFor = team.GoalsFor,
                    GoalsAgainst = team.GoalsAgainst
                };
                copy.Teams.Add(teamCopy);
                teamMapping[team.Id] = teamCopy;
            }
            
            // Копируем матчи
            for (int i = 0; i < Matches.Count; i++)
            {
                var match = Matches[i];
                var matchCopy = new Match
                {
                    Id = match.Id,
                    Round = match.Round,
                    HomeTeam = teamMapping[match.HomeTeam.Id],
                    AwayTeam = teamMapping[match.AwayTeam.Id],
                    HomeGoals = match.HomeGoals,
                    AwayGoals = match.AwayGoals,
                    IsPlayed = match.IsPlayed
                };
                copy.Matches.Add(matchCopy);
            }
            
            // Копируем историю туров
            for (int i = 0; i < RoundHistory.Count; i++)
            {
                var stat = RoundHistory[i];
                copy.RoundHistory.Add(new TeamRoundStats
                {
                    TeamId = stat.TeamId,
                    TeamName = stat.TeamName,
                    Round = stat.Round,
                    Points = stat.Points,
                    Position = stat.Position
                });
            }
            
            return copy;
        }
    }
}
