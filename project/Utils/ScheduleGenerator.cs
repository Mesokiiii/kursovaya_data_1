using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FootballLeague
{
    /// <summary>
    /// Генератор расписания и данных для футбольной лиги
    /// </summary>
    public static class ScheduleGenerator
    {
        /// <summary>
        /// Генерирует полное расписание и сохраняет в JSON
        /// </summary>
        /// <param name="outputPath">Путь для сохранения JSON файла</param>
        /// <param name="teamCount">Количество команд (по умолчанию 20)</param>
        /// <param name="simulateRounds">Количество туров для симуляции (0 = не симулировать)</param>
        public static void GenerateAndSave(string outputPath, int teamCount = 20, int simulateRounds = 0)
        {
            var manager = new LeagueManager();
            manager.GenerateFullSchedule(teamCount);
            
            if (simulateRounds > 0)
            {
                manager.SimulateMatches(simulateRounds);
            }
            
            SaveToJson(manager, outputPath);
        }
        
        /// <summary>
        /// Сохраняет данные лиги в JSON файл (без LINQ)
        /// </summary>
        private static void SaveToJson(LeagueManager manager, string filePath)
        {
            // Преобразуем команды вручную
            List<object> teamsData = new List<object>();
            for (int i = 0; i < manager.Teams.Count; i++)
            {
                Team t = manager.Teams[i];
                teamsData.Add(new
                {
                    id = t.Id,
                    name = t.Name
                });
            }
            
            // Преобразуем матчи вручную
            List<object> matchesData = new List<object>();
            for (int i = 0; i < manager.Matches.Count; i++)
            {
                Match m = manager.Matches[i];
                matchesData.Add(new
                {
                    id = m.Id,
                    round = m.Round,
                    homeTeamId = m.HomeTeam.Id,
                    awayTeamId = m.AwayTeam.Id,
                    homeGoals = m.HomeGoals,
                    awayGoals = m.AwayGoals,
                    isPlayed = m.IsPlayed
                });
            }
            
            var data = new
            {
                teams = teamsData,
                matches = matchesData
            };
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            string json = JsonSerializer.Serialize(data, options);
            
            // Создаем директорию если не существует
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Данные сохранены в {filePath}");
            Console.WriteLine($"Команд: {manager.Teams.Count}");
            Console.WriteLine($"Матчей: {manager.Matches.Count}");
            
            // Подсчитываем сыгранные матчи вручную
            int playedCount = ManualAlgorithms.Count(manager.Matches, m => m.IsPlayed);
            Console.WriteLine($"Сыграно матчей: {playedCount}");
        }
    }
}

