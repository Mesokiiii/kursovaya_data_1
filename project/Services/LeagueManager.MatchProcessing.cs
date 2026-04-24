using System;
using System.Collections.Generic;

namespace FootballLeague
{
    /// <summary>
    /// Часть LeagueManager - обработка матчей и статистики
    /// </summary>
    public partial class LeagueManager
    {
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
        }

        /// <summary>
        /// Обрабатывает все матчи и записывает статистику по турам
        /// ИСПРАВЛЕНИЕ БАГА: Теперь статистика записывается после обработки ВСЕХ матчей каждого тура
        /// </summary>
        public void ProcessAllMatches()
        {
            // Группируем матчи по турам вручную
            List<Match> playedMatches = ManualAlgorithms.Where(Matches, m => m.IsPlayed);
            Dictionary<int, List<Match>> matchesByRound = ManualAlgorithms.GroupBy(playedMatches, m => m.Round);
            
            // Сортируем ключи (номера туров) вручную
            List<int> rounds = new List<int>();
            foreach (var key in matchesByRound.Keys)
            {
                rounds.Add(key);
            }
            rounds = ManualAlgorithms.OrderBy(rounds, r => r);

            // Обрабатываем каждый тур
            for (int i = 0; i < rounds.Count; i++)
            {
                int round = rounds[i];
                List<Match> roundMatches = matchesByRound[round];
                
                // Обрабатываем все матчи тура
                for (int j = 0; j < roundMatches.Count; j++)
                {
                    ProcessMatchResult(roundMatches[j]);
                }
                
                // Записываем статистику только после обработки ВСЕХ матчей тура
                RecordRoundStats(round);
            }
        }

        /// <summary>
        /// Записывает статистику команд на конец тура
        /// </summary>
        /// <param name="round">Номер тура</param>
        private void RecordRoundStats(int round)
        {
            try
            {
                Console.WriteLine($"Запись статистики для тура {round}");
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
                Console.WriteLine($"Статистика для {standings.Count} команд записана");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи статистики тура {round}: {ex.Message}");
            }
        }
    }
}
