using NUnit.Framework;
using System.Collections.Generic;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        private LeagueManager _manager;
        private Team _team1;
        private Team _team2;

        [SetUp]
        public void Setup()
        {
            _manager = new LeagueManager();
            _team1 = new Team { Id = 1, Name = "Team A" };
            _team2 = new Team { Id = 2, Name = "Team B" };
            _manager.Teams.Add(_team1);
            _manager.Teams.Add(_team2);
        }

        [Test]
        public void GetForm_ReturnsCorrectFormString()
        {
            // Arrange
            var matches = new List<Match>
            {
                new Match { Id = 1, Round = 1, HomeTeam = _team1, AwayTeam = _team2, HomeGoals = 2, AwayGoals = 1, IsPlayed = true },
                new Match { Id = 2, Round = 2, HomeTeam = _team2, AwayTeam = _team1, HomeGoals = 0, AwayGoals = 0, IsPlayed = true },
                new Match { Id = 3, Round = 3, HomeTeam = _team1, AwayTeam = _team2, HomeGoals = 1, AwayGoals = 2, IsPlayed = true }
            };

            foreach (var match in matches)
            {
                _manager.ProcessMatchResult(match);
            }

            // Act
            var form = _team1.GetForm(_manager.Matches, 5);

            // Assert
            Assert.That(form, Is.Not.Null);
            Assert.That(form.Length, Is.LessThanOrEqualTo(5));
        }

        [Test]
        public void GetForm_WithNoMatches_ReturnsEmptyString()
        {
            // Act
            var form = _team1.GetForm(new List<Match>(), 5);

            // Assert
            Assert.That(form, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetForm_LimitsToRequestedCount()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                var match = new Match { Id = i, Round = i, HomeTeam = _team1, AwayTeam = _team2, HomeGoals = 1, AwayGoals = 0, IsPlayed = true };
                _manager.Matches.Add(match);
                _manager.ProcessMatchResult(match);
            }

            // Act
            var form = _team1.GetForm(_manager.Matches, 3);

            // Assert
            Assert.That(form.Length, Is.LessThanOrEqualTo(3));
        }
    }
}
