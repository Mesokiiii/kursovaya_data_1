using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague
{
    /// <summary>
    /// Методы расширения для удобства работы с данными
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Получает форму команды (последние N матчей)
        /// </summary>
        /// <param name="team">Команда</param>
        /// <param name="matches">Все матчи</param>
        /// <param name="count">Количество последних матчей</param>
        /// <returns>Строка с результатами (W-победа, D-ничья, L-поражение)</returns>
        public static string GetForm(this Team team, List<Match> matches, int count = 5)
        {
            var teamMatches = matches
                .Where(m => m.IsPlayed && (m.HomeTeam.Id == team.Id || m.AwayTeam.Id == team.Id))
                .OrderByDescending(m => m.Round)
                .Take(count);

            var form = "";
            foreach (var match in teamMatches)
            {
                if (match.HomeTeam.Id == team.Id)
                {
                    if (match.HomeGoals > match.AwayGoals) form += "W";
                    else if (match.HomeGoals < match.AwayGoals) form += "L";
                    else form += "D";
                }
                else
                {
                    if (match.AwayGoals > match.HomeGoals) form += "W";
                    else if (match.AwayGoals < match.HomeGoals) form += "L";
                    else form += "D";
                }
            }

            return form;
        }

        /// <summary>
        /// Проверяет, является ли команда в хорошей форме
        /// </summary>
        /// <param name="team">Команда</param>
        /// <param name="matches">Все матчи</param>
        /// <returns>True, если команда в хорошей форме</returns>
        public static bool IsInGoodForm(this Team team, List<Match> matches)
        {
            var form = team.GetForm(matches, 3);
            var wins = form.Count(c => c == 'W');
            return wins >= 2;
        }

        /// <summary>
        /// Получает процент побед команды
        /// </summary>
        /// <param name="team">Команда</param>
        /// <returns>Процент побед</returns>
        public static double GetWinPercentage(this Team team)
        {
            if (team.Played == 0) return 0;
            return (double)team.Wins / team.Played * 100;
        }

        /// <summary>
        /// Получает среднее количество голов за матч
        /// </summary>
        /// <param name="team">Команда</param>
        /// <returns>Среднее количество голов</returns>
        public static double GetAverageGoalsPerMatch(this Team team)
        {
            if (team.Played == 0) return 0;
            return (double)team.GoalsFor / team.Played;
        }
    }
}