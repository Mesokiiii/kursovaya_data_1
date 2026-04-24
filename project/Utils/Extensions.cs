using System;
using System.Collections.Generic;

namespace FootballLeague
{
    /// <summary>
    /// Методы расширения для удобства работы с данными
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Получает форму команды (последние N матчей) - без LINQ
        /// </summary>
        /// <param name="team">Команда</param>
        /// <param name="matches">Все матчи</param>
        /// <param name="count">Количество последних матчей</param>
        /// <returns>Строка с результатами (W-победа, D-ничья, L-поражение)</returns>
        public static string GetForm(this Team team, List<Match> matches, int count = 5)
        {
            // Фильтруем матчи команды
            List<Match> teamMatches = ManualAlgorithms.Where(matches, 
                m => m.IsPlayed && (m.HomeTeam.Id == team.Id || m.AwayTeam.Id == team.Id));
            
            // Сортируем по убыванию тура
            teamMatches = ManualAlgorithms.OrderByDescending(teamMatches, m => m.Round);
            
            // Берем первые count матчей
            teamMatches = ManualAlgorithms.Take(teamMatches, count);

            string form = "";
            for (int i = 0; i < teamMatches.Count; i++)
            {
                Match match = teamMatches[i];
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
        /// Проверяет, является ли команда в хорошей форме (без LINQ)
        /// </summary>
        /// <param name="team">Команда</param>
        /// <param name="matches">Все матчи</param>
        /// <returns>True, если команда в хорошей форме</returns>
        public static bool IsInGoodForm(this Team team, List<Match> matches)
        {
            string form = team.GetForm(matches, 3);
            
            // Подсчитываем победы вручную
            int wins = 0;
            for (int i = 0; i < form.Length; i++)
            {
                if (form[i] == 'W')
                    wins++;
            }
            
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