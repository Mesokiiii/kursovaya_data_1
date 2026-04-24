using NUnit.Framework;
using FootballLeague;

namespace FootballLeague.Tests
{
    /// <summary>
    /// Тесты для класса Team
    /// </summary>
    [TestFixture]
    public class TeamTests
    {
        [Test]
        public void Team_CalculatesPointsCorrectly()
        {
            // Arrange
            var team = new Team
            {
                Id = 1,
                Name = "Тестовая команда",
                Wins = 10,
                Draws = 5,
                Losses = 3
            };

            // Act
            int points = team.Points;

            // Assert
            Assert.That(points, Is.EqualTo(35)); // 10*3 + 5*1 = 35
        }

        [Test]
        public void Team_CalculatesGoalDifferenceCorrectly()
        {
            // Arrange
            var team = new Team
            {
                Id = 1,
                Name = "Тестовая команда",
                GoalsFor = 50,
                GoalsAgainst = 30
            };

            // Act
            int goalDifference = team.GoalDifference;

            // Assert
            Assert.That(goalDifference, Is.EqualTo(20));
        }

        [Test]
        public void Team_IsValid_ReturnsTrueForValidTeam()
        {
            // Arrange
            var team = new Team
            {
                Id = 1,
                Name = "Валидная команда",
                Played = 10,
                Wins = 5,
                Draws = 3,
                Losses = 2,
                GoalsFor = 20,
                GoalsAgainst = 15
            };

            // Act
            bool isValid = team.IsValid();

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void Team_IsValid_ReturnsFalseWhenPlayedDoesNotMatchResults()
        {
            // Arrange
            var team = new Team
            {
                Id = 1,
                Name = "Невалидная команда",
                Played = 10,
                Wins = 5,
                Draws = 3,
                Losses = 3, // 5+3+3 = 11, не равно Played
                GoalsFor = 20,
                GoalsAgainst = 15
            };

            // Act
            bool isValid = team.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void Team_IsValid_ReturnsFalseForEmptyName()
        {
            // Arrange
            var team = new Team
            {
                Id = 1,
                Name = "",
                Played = 10,
                Wins = 5,
                Draws = 3,
                Losses = 2
            };

            // Act
            bool isValid = team.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
