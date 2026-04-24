using NUnit.Framework;
using FootballLeague;
using System.Collections.Generic;

namespace FootballLeague.Tests
{
    /// <summary>
    /// Тесты для класса ManualAlgorithms
    /// </summary>
    [TestFixture]
    public class ManualAlgorithmsTests
    {
        [Test]
        public void Any_ReturnsTrue_WhenCollectionHasElements()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            bool result = ManualAlgorithms.Any(list);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Any_ReturnsFalse_WhenCollectionIsEmpty()
        {
            // Arrange
            var list = new List<int>();

            // Act
            bool result = ManualAlgorithms.Any(list);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Where_FiltersCorrectly()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 5 },
                new Team { Id = 3, Name = "Team3", Wins = 15 }
            };

            // Act
            var result = ManualAlgorithms.Where(teams, t => t.Wins > 7);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Team1"));
            Assert.That(result[1].Name, Is.EqualTo("Team3"));
        }

        [Test]
        public void FirstOrDefault_ReturnsFirstMatchingElement()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 5 },
                new Team { Id = 3, Name = "Team3", Wins = 15 }
            };

            // Act
            var result = ManualAlgorithms.FirstOrDefault(teams, t => t.Wins > 7);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Team1"));
        }

        [Test]
        public void FirstOrDefault_ReturnsNull_WhenNoMatch()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 5 }
            };

            // Act
            var result = ManualAlgorithms.FirstOrDefault(teams, t => t.Wins > 20);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Count_ReturnsCorrectCount()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 5 },
                new Team { Id = 3, Name = "Team3", Wins = 15 }
            };

            // Act
            int count = ManualAlgorithms.Count(teams, t => t.Wins > 7);

            // Assert
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void OrderBy_SortsAscending()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 5 },
                new Team { Id = 3, Name = "Team3", Wins = 15 }
            };

            // Act
            var result = ManualAlgorithms.OrderBy(teams, t => t.Wins);

            // Assert
            Assert.That(result[0].Wins, Is.EqualTo(5));
            Assert.That(result[1].Wins, Is.EqualTo(10));
            Assert.That(result[2].Wins, Is.EqualTo(15));
        }

        [Test]
        public void OrderByDescending_SortsDescending()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 5 },
                new Team { Id = 3, Name = "Team3", Wins = 15 }
            };

            // Act
            var result = ManualAlgorithms.OrderByDescending(teams, t => t.Wins);

            // Assert
            Assert.That(result[0].Wins, Is.EqualTo(15));
            Assert.That(result[1].Wins, Is.EqualTo(10));
            Assert.That(result[2].Wins, Is.EqualTo(5));
        }

        [Test]
        public void Take_ReturnCorrectNumberOfElements()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1" },
                new Team { Id = 2, Name = "Team2" },
                new Team { Id = 3, Name = "Team3" },
                new Team { Id = 4, Name = "Team4" },
                new Team { Id = 5, Name = "Team5" }
            };

            // Act
            var result = ManualAlgorithms.Take(teams, 3);

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("Team1"));
            Assert.That(result[2].Name, Is.EqualTo("Team3"));
        }

        [Test]
        public void GroupBy_GroupsCorrectly()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1, Name = "Team1", Wins = 10 },
                new Team { Id = 2, Name = "Team2", Wins = 10 },
                new Team { Id = 3, Name = "Team3", Wins = 5 }
            };

            // Act
            var result = ManualAlgorithms.GroupBy(teams, t => t.Wins);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[10].Count, Is.EqualTo(2));
            Assert.That(result[5].Count, Is.EqualTo(1));
        }
    }
}
