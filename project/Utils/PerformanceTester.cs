using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FootballLeague
{
    /// <summary>
    /// Класс для тестирования производительности алгоритмов сортировки
    /// </summary>
    public static class PerformanceTester
    {
        /// <summary>
        /// Генерирует случайные команды для тестирования
        /// </summary>
        /// <param name="count">Количество команд</param>
        /// <returns>Список команд</returns>
        public static List<Team> GenerateRandomTeams(int count)
        {
            Random random = new Random();
            List<Team> teams = new List<Team>();
            
            for (int i = 0; i < count; i++)
            {
                teams.Add(new Team
                {
                    Id = i + 1,
                    Name = $"Команда_{i + 1}",
                    Played = random.Next(0, 38),
                    Wins = random.Next(0, 20),
                    Draws = random.Next(0, 10),
                    Losses = random.Next(0, 20),
                    GoalsFor = random.Next(0, 100),
                    GoalsAgainst = random.Next(0, 100)
                });
            }
            
            return teams;
        }

        /// <summary>
        /// Сортировка пузырьком (Bubble Sort) - простой алгоритм O(n²)
        /// </summary>
        public static List<Team> BubbleSort(List<Team> teams)
        {
            List<Team> result = new List<Team>(teams);
            int n = result.Count;
            
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Сравниваем по очкам (по убыванию)
                    if (result[j].Points < result[j + 1].Points)
                    {
                        Team temp = result[j];
                        result[j] = result[j + 1];
                        result[j + 1] = temp;
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// Сортировка вставками (Insertion Sort) - простой алгоритм O(n²)
        /// </summary>
        public static List<Team> InsertionSort(List<Team> teams)
        {
            List<Team> result = new List<Team>(teams);
            int n = result.Count;
            
            for (int i = 1; i < n; i++)
            {
                Team key = result[i];
                int j = i - 1;
                
                // Сравниваем по очкам (по убыванию)
                while (j >= 0 && result[j].Points < key.Points)
                {
                    result[j + 1] = result[j];
                    j--;
                }
                result[j + 1] = key;
            }
            
            return result;
        }

        /// <summary>
        /// Измеряет время выполнения сортировки
        /// </summary>
        /// <param name="teams">Список команд</param>
        /// <param name="sortMethod">Метод сортировки</param>
        /// <returns>Время выполнения в миллисекундах</returns>
        public static long MeasureSortTime(List<Team> teams, Func<List<Team>, List<Team>> sortMethod)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            sortMethod(teams);
            
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Запускает полное тестирование производительности
        /// </summary>
        public static void RunPerformanceTests()
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ ПРОИЗВОДИТЕЛЬНОСТИ АЛГОРИТМОВ СОРТИРОВКИ ===\n");
            
            int[] testSizes = { 100, 1000, 10000 };
            
            foreach (int size in testSizes)
            {
                Console.WriteLine($"--- Тестирование на {size} элементах ---");
                
                // Генерируем данные
                List<Team> teams = GenerateRandomTeams(size);
                
                // Тест 1: QuickSort (наш оптимизированный алгоритм)
                long quickSortTime = MeasureSortTime(teams, t => 
                    ManualAlgorithms.OrderByDescending(t, team => team.Points));
                Console.WriteLine($"QuickSort:        {quickSortTime} мс");
                
                // Тест 2: Bubble Sort (простой алгоритм)
                long bubbleSortTime = MeasureSortTime(teams, BubbleSort);
                Console.WriteLine($"Bubble Sort:      {bubbleSortTime} мс");
                
                // Тест 3: Insertion Sort (простой алгоритм)
                long insertionSortTime = MeasureSortTime(teams, InsertionSort);
                Console.WriteLine($"Insertion Sort:   {insertionSortTime} мс");
                
                // Вычисляем ускорение
                if (bubbleSortTime > 0)
                {
                    double speedupBubble = (double)bubbleSortTime / quickSortTime;
                    Console.WriteLine($"\nУскорение QuickSort vs Bubble Sort:    {speedupBubble:F2}x");
                }
                
                if (insertionSortTime > 0)
                {
                    double speedupInsertion = (double)insertionSortTime / quickSortTime;
                    Console.WriteLine($"Ускорение QuickSort vs Insertion Sort: {speedupInsertion:F2}x");
                }
                
                Console.WriteLine();
            }
            
            Console.WriteLine("=== ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===\n");
        }

        /// <summary>
        /// Запускает детальное тестирование с многоуровневой сортировкой
        /// </summary>
        public static void RunDetailedPerformanceTests()
        {
            Console.WriteLine("=== ДЕТАЛЬНОЕ ТЕСТИРОВАНИЕ МНОГОУРОВНЕВОЙ СОРТИРОВКИ ===\n");
            
            int[] testSizes = { 100, 1000, 5000 };
            
            foreach (int size in testSizes)
            {
                Console.WriteLine($"--- Тестирование на {size} элементах ---");
                
                List<Team> teams = GenerateRandomTeams(size);
                
                // Многоуровневая сортировка (как в реальном приложении)
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                
                List<Comparison<Team>> comparisons = new List<Comparison<Team>>
                {
                    (a, b) => b.Points.CompareTo(a.Points),
                    (a, b) => b.GoalDifference.CompareTo(a.GoalDifference),
                    (a, b) => b.GoalsFor.CompareTo(a.GoalsFor),
                    (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal)
                };
                
                ManualAlgorithms.MultiSort(teams, comparisons);
                
                stopwatch.Stop();
                Console.WriteLine($"Многоуровневая сортировка (MergeSort): {stopwatch.ElapsedMilliseconds} мс");
                
                // Простая сортировка для сравнения
                stopwatch.Restart();
                BubbleSort(teams);
                stopwatch.Stop();
                Console.WriteLine($"Простая сортировка (Bubble Sort):      {stopwatch.ElapsedMilliseconds} мс");
                
                Console.WriteLine();
            }
            
            Console.WriteLine("=== ДЕТАЛЬНОЕ ТЕСТИРОВАНИЕ ЗАВЕРШЕНО ===\n");
        }

        /// <summary>
        /// Тестирует производительность фильтрации
        /// </summary>
        public static void RunFilterPerformanceTests()
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ ПРОИЗВОДИТЕЛЬНОСТИ ФИЛЬТРАЦИИ ===\n");
            
            int[] testSizes = { 1000, 10000, 50000 };
            
            foreach (int size in testSizes)
            {
                Console.WriteLine($"--- Тестирование на {size} элементах ---");
                
                List<Team> teams = GenerateRandomTeams(size);
                
                // Тест фильтрации
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                
                List<Team> filtered = ManualAlgorithms.Where(teams, t => t.Points > 30);
                
                stopwatch.Stop();
                Console.WriteLine($"Фильтрация (Where):     {stopwatch.ElapsedMilliseconds} мс");
                Console.WriteLine($"Найдено элементов:      {filtered.Count}");
                
                // Тест поиска
                stopwatch.Restart();
                Team found = ManualAlgorithms.FirstOrDefault(teams, t => t.Points > 50);
                stopwatch.Stop();
                Console.WriteLine($"Поиск (FirstOrDefault): {stopwatch.ElapsedMilliseconds} мс");
                Console.WriteLine($"Найден элемент:         {(found != null ? "Да" : "Нет")}");
                
                Console.WriteLine();
            }
            
            Console.WriteLine("=== ТЕСТИРОВАНИЕ ФИЛЬТРАЦИИ ЗАВЕРШЕНО ===\n");
        }

        /// <summary>
        /// Запускает все тесты производительности
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  КОМПЛЕКСНОЕ ТЕСТИРОВАНИЕ ПРОИЗВОДИТЕЛЬНОСТИ АЛГОРИТМОВ   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
            
            RunPerformanceTests();
            RunDetailedPerformanceTests();
            RunFilterPerformanceTests();
            
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              ВСЕ ТЕСТЫ УСПЕШНО ЗАВЕРШЕНЫ                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }
    }
}
