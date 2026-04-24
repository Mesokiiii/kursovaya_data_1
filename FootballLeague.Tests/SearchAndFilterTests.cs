using NUnit.Framework;
using FootballLeague;
using System.Collections.Generic;

namespace FootballLeague.Tests
{
    /// <summary>
    /// Тесты для поиска и фильтрации
    /// </summary>
    [TestFixture]
    public class SearchAndFilterTests
    {
        private LeagueManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new LeagueManager();
            _manager.Teams.Add(new Team 
            { 
                Id = 1, 
                Name = "Спартак", 
                Played = 10,
                Wins = 7, 
                Draws = 2, 
                Losses = 1, 
                GoalsFor = 25, 
                GoalsAgainst = 10 
            });
            _manager.Teams.Add(new Team 
            { 
                Id = 2, 
                Name = "ЦСКА", 
                Played = 10,
                Wins = 5, 
                Draws = 3, 
                Losses = 2, 
                GoalsFor = 18, 
                GoalsAgainst = 12 
            });
            _manager.Teams.Add(new Team 
            { 
                Id = 3, 
                Name = "Динамо", 
                Played = 10,
                Wins = 3, 
                Draws = 4, 
                Losses = 3, 
                GoalsFor = 12, 
                GoalsAgainst = 15 
            });
        }

        [Test]
        public void SearchTeamsByCriteria_FindsTeamsByMinPoints()
        {
            // Arrange
            var criteria = new LeagueManager.SearchCriteria
            {
                MinPoints = 20
            };

            // Act
            var result = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Спартак"));
        }

        [Test]
        public void SearchTeamsByCriteria_FindsTeamsByMinGoalsFor()
        {
            // Arrange
            var criteria = new LeagueManager.SearchCriteria
            {
                MinGoalsFor = 18
            };

            // Act
            var result = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void SearchTeamsByCriteria_FindsTeamsByNameContains()
        {
            // Arrange
            var criteria = new LeagueManager.SearchCriteria
            {
                NameContains = "СКА"
            };

            // Act
            var result = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("ЦСКА"));
        }

        [Test]
        public void SearchTeamsByCriteria_FindsTeamsByMultipleCriteria()
        {
            // Arrange
            var criteria = new LeagueManager.SearchCriteria
            {
                MinWins = 5,
                MinGoalsFor = 18
            };

            // Act
            var result = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void SearchTeamsByCriteria_ReturnsEmptyWhenNoCriteria()
        {
            // Arrange
            var criteria = new LeagueManager.SearchCriteria
            {
                MinPoints = 100 // Нереальное значение
            };

            // Act
            var result = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
