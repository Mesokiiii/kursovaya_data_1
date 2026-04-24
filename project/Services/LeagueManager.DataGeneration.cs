using System;
using System.Collections.Generic;

namespace FootballLeague
{
    /// <summary>
    /// Часть LeagueManager - генерация данных и симуляция
    /// </summary>
    public partial class LeagueManager
    {
        /// <summary>
        /// Генерирует тестовые данные для демонстрации
        /// </summary>
        public void GenerateMockData()
        {
            // Создаем тестовые команды
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Спартак" },
                new Team { Id = 2, Name = "ЦСКА" },
                new Team { Id = 3, Name = "Локомотив" },
                new Team { Id = 4, Name = "Динамо" },
                new Team { Id = 5, Name = "Зенит" }
            };

            Teams = teams;

            // Создаем тестовые матчи
            var matches = new List<Match>
            {
                new Match { Id = 1, HomeTeam = teams[0], AwayTeam = teams[1], HomeGoals = 2, AwayGoals = 1, IsPlayed = true, Round = 1 },
                new Match { Id = 2, HomeTeam = teams[2], AwayTeam = teams[3], HomeGoals = 1, AwayGoals = 1, IsPlayed = true, Round = 1 },
                new Match { Id = 3, HomeTeam = teams[4], AwayTeam = teams[0], HomeGoals = 0, AwayGoals = 3, IsPlayed = true, Round = 2 },
                new Match { Id = 4, HomeTeam = teams[1], AwayTeam = teams[2], HomeGoals = 2, AwayGoals = 0, IsPlayed = true, Round = 2 }
            };

            Matches = matches;

            // Обрабатываем все матчи
            ProcessAllMatches();
        }

        /// <summary>
        /// Генерирует полное расписание для лиги (круговая система)
        /// Для 20 команд создается 380 матчей (каждая команда играет с каждой дома и в гостях)
        /// </summary>
        /// <param name="teamCount">Количество команд (по умолчанию 20)</param>
        public void GenerateFullSchedule(int teamCount = 20)
        {
            if (teamCount < Constants.MIN_TEAMS_IN_LEAGUE || teamCount > Constants.MAX_TEAMS_IN_LEAGUE)
            {
                throw new ArgumentException($"Количество команд должно быть от {Constants.MIN_TEAMS_IN_LEAGUE} до {Constants.MAX_TEAMS_IN_LEAGUE}");
            }

            // Генерируем команды
            Teams.Clear();
            string[] teamNames = {
                "Спартак", "ЦСКА", "Локомотив", "Динамо", "Зенит",
                "Краснодар", "Рубин", "Ростов", "Урал", "Ахмат",
                "Сочи", "Крылья Советов", "Оренбург", "Факел", "Балтика",
                "Нижний Новгород", "Химки", "Торпедо", "Пари НН", "Акрон"
            };

            for (int i = 0; i < teamCount; i++)
            {
                Teams.Add(new Team
                {
                    Id = i + 1,
                    Name = i < teamNames.Length ? teamNames[i] : $"Команда {i + 1}"
                });
            }

            // Генерируем расписание по алгоритму круговой системы
            Matches.Clear();
            int matchId = 1;
            int totalRounds = (teamCount - 1) * 2; // Два круга (дома и в гостях)

            // Первый круг (каждая команда играет дома с каждой)
            for (int round = 1; round <= teamCount - 1; round++)
            {
                for (int i = 0; i < teamCount / 2; i++)
                {
                    int home = (round + i - 1) % (teamCount - 1);
                    int away = (teamCount - 1 - i + round - 1) % (teamCount - 1);

                    // Последняя команда всегда фиксирована
                    if (i == 0)
                    {
                        away = teamCount - 1;
                    }

                    // Чередуем дома/гости для баланса
                    if (round % 2 == 1)
                    {
                        Matches.Add(new Match
                        {
                            Id = matchId++,
                            Round = round,
                            HomeTeam = Teams[home],
                            AwayTeam = Teams[away],
                            IsPlayed = false
                        });
                    }
                    else
                    {
                        Matches.Add(new Match
                        {
                            Id = matchId++,
                            Round = round,
                            HomeTeam = Teams[away],
                            AwayTeam = Teams[home],
                            IsPlayed = false
                        });
                    }
                }
            }

            // Второй круг (зеркальные матчи)
            int firstCircleMatches = Matches.Count;
            for (int i = 0; i < firstCircleMatches; i++)
            {
                var originalMatch = Matches[i];
                Matches.Add(new Match
                {
                    Id = matchId++,
                    Round = originalMatch.Round + (teamCount - 1),
                    HomeTeam = originalMatch.AwayTeam,
                    AwayTeam = originalMatch.HomeTeam,
                    IsPlayed = false
                });
            }

            Console.WriteLine($"Создано {Teams.Count} команд и {Matches.Count} матчей в {totalRounds} турах");
        }

        /// <summary>
        /// Симулирует случайные результаты для всех матчей (без LINQ)
        /// </summary>
        /// <param name="upToRound">До какого тура симулировать (0 = все туры)</param>
        public void SimulateMatches(int upToRound = 0)
        {
            var random = new Random();
            
            // Фильтруем матчи вручную
            List<Match> matchesToSimulate = new List<Match>();
            for (int i = 0; i < Matches.Count; i++)
            {
                Match match = Matches[i];
                if (!match.IsPlayed)
                {
                    if (upToRound > 0)
                    {
                        if (match.Round <= upToRound)
                            matchesToSimulate.Add(match);
                    }
                    else
                    {
                        matchesToSimulate.Add(match);
                    }
                }
            }

            // Симулируем результаты
            for (int i = 0; i < matchesToSimulate.Count; i++)
            {
                // Генерируем реалистичные счета (0-5 голов)
                matchesToSimulate[i].HomeGoals = random.Next(0, 6);
                matchesToSimulate[i].AwayGoals = random.Next(0, 6);
                matchesToSimulate[i].IsPlayed = true;
            }

            // Обрабатываем все сыгранные матчи
            ProcessAllMatches();
        }
    }
}
