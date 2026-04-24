using NUnit.Framework;
using System;
using System.Collections.Generic;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ManualAlgorithmsExtendedTests
    {
        [Test]
        public void Where_WithPredicate_FiltersCorrectly()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Act
            var result = ManualAlgorithms.Where(list, x => x > 5);

            // Assert
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result[0], Is.EqualTo(6));
        }

        [Test]
        public void FirstOrDefault_FindsElement()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = ManualAlgorithms.FirstOrDefault(list, x => x == 3);

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void FirstOrDefault_NotFound_ReturnsDefault()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = ManualAlgorithms.FirstOrDefault(list, x => x == 10);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Any_WithMatchingElement_ReturnsTrue()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = ManualAlgorithms.Any(list, x => x > 3);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Any_WithNoMatch_ReturnsFalse()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            var result = ManualAlgorithms.Any(list, x => x > 10);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Count_WithPredicate_ReturnsCorrectCount()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // Act
            var result = ManualAlgorithms.Count(list, x => x % 2 == 0);

            // Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Select_TransformsElements()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = ManualAlgorithms.Select(list, x => x * 2);

            // Assert
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result[0], Is.EqualTo(2));
            Assert.That(result[4], Is.EqualTo(10));
        }

        [Test]
        public void OrderBy_SortsAscending()
        {
            // Arrange
            var list = new List<int> { 5, 2, 8, 1, 9, 3 };

            // Act
            var result = ManualAlgorithms.OrderBy(list, x => x);

            // Assert
            Assert.That(result[0], Is.EqualTo(1));
            Assert.That(result[5], Is.EqualTo(9));
        }

        [Test]
        public void OrderByDescending_SortsDescending()
        {
            // Arrange
            var list = new List<int> { 5, 2, 8, 1, 9, 3 };

            // Act
            var result = ManualAlgorithms.OrderByDescending(list, x => x);

            // Assert
            Assert.That(result[0], Is.EqualTo(9));
            Assert.That(result[5], Is.EqualTo(1));
        }

        [Test]
        public void MultiSort_SortsWithMultipleCriteria()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "A", Wins = 3, Draws = 1, GoalsFor = 10, GoalsAgainst = 5 },
                new Team { Id = 2, Name = "B", Wins = 3, Draws = 1, GoalsFor = 8, GoalsAgainst = 5 },
                new Team { Id = 3, Name = "C", Wins = 4, Draws = 0, GoalsFor = 12, GoalsAgainst = 10 }
            };

            var comparisons = new List<Comparison<Team>>
            {
                (a, b) => b.Points.CompareTo(a.Points),
                (a, b) => b.GoalDifference.CompareTo(a.GoalDifference)
            };

            // Act
            var result = ManualAlgorithms.MultiSort(teams, comparisons);

            // Assert
            Assert.That(result[0].Id, Is.EqualTo(3)); // 12 очков
            Assert.That(result[1].Id, Is.EqualTo(1)); // 10 очков, разница 5
            Assert.That(result[2].Id, Is.EqualTo(2)); // 10 очков, разница 3
        }

        [Test]
        public void Where_EmptyList_ReturnsEmpty()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = ManualAlgorithms.Where(list, x => x > 0);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void OrderBy_EmptyList_ReturnsEmpty()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = ManualAlgorithms.OrderBy(list, x => x);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Select_EmptyList_ReturnsEmpty()
        {
            // Arrange
            var list = new List<int>();

            // Act
            var result = ManualAlgorithms.Select(list, x => x * 2);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
