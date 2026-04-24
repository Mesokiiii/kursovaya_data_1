using System;
using System.Collections.Generic;

namespace FootballLeague
{
    /// <summary>
    /// Часть LeagueManager - поиск и фильтрация команд
    /// </summary>
    public partial class LeagueManager
    {
        /// <summary>
        /// Получает матчи между двумя командами
        /// </summary>
        /// <param name="team1">Первая команда</param>
        /// <param name="team2">Вторая команда</param>
        /// <returns>Список матчей между командами</returns>
        public List<Match> GetHeadToHead(Team team1, Team team2)
        {
            if (team1 == null || team2 == null)
                return new List<Match>();

            return ManualAlgorithms.Where(Matches, m =>
                (m.HomeTeam == team1 && m.AwayTeam == team2) ||
                (m.HomeTeam == team2 && m.AwayTeam == team1)
            );
        }

        /// <summary>
        /// Поиск команд по условию
        /// </summary>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Список найденных команд</returns>
        public List<Team> SearchTeams(Func<Team, bool> predicate)
        {
            if (predicate == null)
                return new List<Team>();

            return ManualAlgorithms.Where(Teams, predicate);
        }

        /// <summary>
        /// Критерии поиска команд
        /// </summary>
        public class SearchCriteria
        {
            public int? MinGoalsFor { get; set; }
            public int? MaxGoalsFor { get; set; }
            public int? MinGoalsAgainst { get; set; }
            public int? MaxGoalsAgainst { get; set; }
            public int? MinPoints { get; set; }
            public int? MaxPoints { get; set; }
            public int? MinWins { get; set; }
            public int? MinDraws { get; set; }
            public int? MinLosses { get; set; }
            public int? MinGoalDifference { get; set; }
            public int? MaxGoalDifference { get; set; }
            public string NameContains { get; set; }
        }

        /// <summary>
        /// Расширенный поиск команд по множественным критериям (без LINQ)
        /// </summary>
        public List<Team> SearchTeamsByCriteria(SearchCriteria criteria)
        {
            if (criteria == null)
                return new List<Team>(Teams);

            List<Team> result = new List<Team>(Teams);

            // Применяем фильтры последовательно
            if (criteria.MinGoalsFor.HasValue)
                result = ManualAlgorithms.Where(result, t => t.GoalsFor >= criteria.MinGoalsFor.Value);

            if (criteria.MaxGoalsFor.HasValue)
                result = ManualAlgorithms.Where(result, t => t.GoalsFor <= criteria.MaxGoalsFor.Value);

            if (criteria.MinGoalsAgainst.HasValue)
                result = ManualAlgorithms.Where(result, t => t.GoalsAgainst >= criteria.MinGoalsAgainst.Value);

            if (criteria.MaxGoalsAgainst.HasValue)
                result = ManualAlgorithms.Where(result, t => t.GoalsAgainst <= criteria.MaxGoalsAgainst.Value);

            if (criteria.MinPoints.HasValue)
                result = ManualAlgorithms.Where(result, t => t.Points >= criteria.MinPoints.Value);

            if (criteria.MaxPoints.HasValue)
                result = ManualAlgorithms.Where(result, t => t.Points <= criteria.MaxPoints.Value);

            if (criteria.MinWins.HasValue)
                result = ManualAlgorithms.Where(result, t => t.Wins >= criteria.MinWins.Value);

            if (criteria.MinDraws.HasValue)
                result = ManualAlgorithms.Where(result, t => t.Draws >= criteria.MinDraws.Value);

            if (criteria.MinLosses.HasValue)
                result = ManualAlgorithms.Where(result, t => t.Losses >= criteria.MinLosses.Value);

            if (criteria.MinGoalDifference.HasValue)
                result = ManualAlgorithms.Where(result, t => t.GoalDifference >= criteria.MinGoalDifference.Value);

            if (criteria.MaxGoalDifference.HasValue)
                result = ManualAlgorithms.Where(result, t => t.GoalDifference <= criteria.MaxGoalDifference.Value);

            if (!string.IsNullOrWhiteSpace(criteria.NameContains))
                result = ManualAlgorithms.Where(result, t => t.Name.IndexOf(criteria.NameContains, StringComparison.OrdinalIgnoreCase) >= 0);

            return result;
        }
    }
}
