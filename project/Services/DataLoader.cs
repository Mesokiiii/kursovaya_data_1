using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace FootballLeague
{
    public class DataLoader
    {
        public static void LoadFromJson(LeagueManager manager, string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;

                    // Загружаем команды
                    if (root.TryGetProperty("teams", out JsonElement teamsArray))
                    {
                        foreach (JsonElement teamElement in teamsArray.EnumerateArray())
                        {
                            int id = teamElement.GetProperty("id").GetInt32();
                            string name = teamElement.GetProperty("name").GetString();
                            manager.Teams.Add(new Team { Id = id, Name = name });
                        }
                    }

                    // Загружаем матчи
                    if (root.TryGetProperty("matches", out JsonElement matchesArray))
                    {
                        foreach (JsonElement matchElement in matchesArray.EnumerateArray())
                        {
                            int id = matchElement.GetProperty("id").GetInt32();
                            int round = matchElement.GetProperty("round").GetInt32();
                            int homeTeamId = matchElement.GetProperty("homeTeamId").GetInt32();
                            int awayTeamId = matchElement.GetProperty("awayTeamId").GetInt32();
                            int homeGoals = matchElement.GetProperty("homeGoals").GetInt32();
                            int awayGoals = matchElement.GetProperty("awayGoals").GetInt32();
                            bool isPlayed = matchElement.GetProperty("isPlayed").GetBoolean();

                            Team homeTeam = manager.Teams.FirstOrDefault(t => t.Id == homeTeamId);
                            Team awayTeam = manager.Teams.FirstOrDefault(t => t.Id == awayTeamId);

                            if (homeTeam != null && awayTeam != null)
                            {
                                var match = new Match
                                {
                                    Id = id,
                                    Round = round,
                                    HomeTeam = homeTeam,
                                    AwayTeam = awayTeam,
                                    HomeGoals = homeGoals,
                                    AwayGoals = awayGoals,
                                    IsPlayed = isPlayed
                                };
                                manager.Matches.Add(match);
                                if (isPlayed)
                                {
                                    manager.ProcessMatchResult(match);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при загрузке данных из JSON: {ex.Message}");
            }
        }
    }
}
