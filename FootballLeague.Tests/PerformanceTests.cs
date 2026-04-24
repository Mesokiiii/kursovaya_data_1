using NUnit.Framework;
using FootballLeague;
using System.Collections.Generic;
using System.Diagnostics;

namespace FootballLeague.Tests
{
    /// <summary>
    /// Тесты производительности алгоритмов
    /// </summary>
    [TestFixture]
    public class PerformanceTests
    {
        [Test]
        public void QuickSort_IsFasterThanBubbleSort_For1000Elements()
        {
            // Arrange
            var teams = PerformanceTester.GenerateRandomTeams(1000);

            // Act
            long quickSortTime = PerformanceTester.MeasureSortTime(teams, 
                t => ManualAlgorithms.OrderByDescending(t, team => team.Points));
            
            long bubbleSortTime = PerformanceTester.MeasureSortTime(teams, 
                PerformanceTester.BubbleSort);

            // Assert
            Assert.That(quickSortTime, Is.LessThan(bubbleSortTime));
            TestContext.WriteLine($"QuickSort: {quickSortTime}ms, BubbleSort: {bubbleSortTime}ms");
            TestContext.WriteLine($"Ускорение: {(double)bubbleSortTime / quickSortTime:F2}x");
        }

        [Test]
        public void QuickSort_IsFasterThanInsertionSort_For1000Elements()
        {
            // Arrange
            var teams = PerformanceTester.GenerateRandomTeams(1000);

            // Act
            long quickSortTime = PerformanceTester.MeasureSortTime(teams, 
                t => ManualAlgorithms.OrderByDescending(t, team => team.Points));
            
            long insertionSortTime = PerformanceTester.MeasureSortTime(teams, 
                PerformanceTester.InsertionSort);

            // Assert
            Assert.That(quickSortTime, Is.LessThan(insertionSortTime));
            TestContext.WriteLine($"QuickSort: {quickSortTime}ms, InsertionSort: {insertionSortTime}ms");
            TestContext.WriteLine($"Ускорение: {(double)insertionSortTime / quickSortTime:F2}x");
        }

        [Test]
        public void BubbleSort_SortsCorrectly()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 5, Draws = 0, Losses = 0 },
                new Team { Id = 2, Name = "Team2", Wins = 10, Draws = 0, Losses = 0 },
                new Team { Id = 3, Name = "Team3", Wins = 3, Draws = 0, Losses = 0 }
            };

            // Act
            var sorted = PerformanceTester.BubbleSort(teams);

            // Assert
            Assert.That(sorted[0].Points, Is.EqualTo(30)); // 10 wins
            Assert.That(sorted[1].Points, Is.EqualTo(15)); // 5 wins
            Assert.That(sorted[2].Points, Is.EqualTo(9));  // 3 wins
        }

        [Test]
        public void InsertionSort_SortsCorrectly()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 5, Draws = 0, Losses = 0 },
                new Team { Id = 2, Name = "Team2", Wins = 10, Draws = 0, Losses = 0 },
                new Team { Id = 3, Name = "Team3", Wins = 3, Draws = 0, Losses = 0 }
            };

            // Act
            var sorted = PerformanceTester.InsertionSort(teams);

            // Assert
            Assert.That(sorted[0].Points, Is.EqualTo(30)); // 10 wins
            Assert.That(sorted[1].Points, Is.EqualTo(15)); // 5 wins
            Assert.That(sorted[2].Points, Is.EqualTo(9));  // 3 wins
        }

        [Test]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(5000)]
        public void QuickSort_HandlesLargeDatasets(int size)
        {
            // Arrange
            var teams = PerformanceTester.GenerateRandomTeams(size);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var sorted = ManualAlgorithms.OrderByDescending(teams, t => t.Points);
            stopwatch.Stop();

            // Assert
            Assert.That(sorted.Count, Is.EqualTo(size));
            TestContext.WriteLine($"Сортировка {size} элементов: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
