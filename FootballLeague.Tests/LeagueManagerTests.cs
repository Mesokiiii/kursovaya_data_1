using NUnit.Framework;
using FootballLeague;
using System.Collections.Generic;

namespace FootballLeague.Tests
{
    /// <summary>
    /// Тесты для класса LeagueManager
    /// </summary>
    [TestFixture]
    public class LeagueManagerTests
    {
        private LeagueManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new LeagueManager();
        }

        [Test]
        public void ProcessMatchResult_UpdatesTeamStatistics()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Home Team" };
            var awayTeam = new Team { Id = 2, Name = "Away Team" };
            var match = new Match
            {
                Id = 1,
                Round = 1,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeGoals = 3,
                AwayGoals = 1,
                IsPlayed = true
            };

            // Act
            _manager.ProcessMatchResult(match);

            // Assert
            Assert.That(homeTeam.Played, Is.EqualTo(1));
            Assert.That(homeTeam.Wins, Is.EqualTo(1));
            Assert.That(homeTeam.GoalsFor, Is.EqualTo(3));
            Assert.That(homeTeam.GoalsAgainst, Is.EqualTo(1));
            
            Assert.That(awayTeam.Played, Is.EqualTo(1));
            Assert.That(awayTeam.Losses, Is.EqualTo(1));
            Assert.That(awayTeam.GoalsFor, Is.EqualTo(1));
            Assert.That(awayTeam.GoalsAgainst, Is.EqualTo(3));
        }

        [Test]
        public void ProcessMatchResult_HandlesDrawCorrectly()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Home Team" };
            var awayTeam = new Team { Id = 2, Name = "Away Team" };
            var match = new Match
            {
                Id = 1,
                Round = 1,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeGoals = 2,
                AwayGoals = 2,
                IsPlayed = true
            };

            // Act
            _manager.ProcessMatchResult(match);

            // Assert
            Assert.That(homeTeam.Draws, Is.EqualTo(1));
            Assert.That(awayTeam.Draws, Is.EqualTo(1));
            Assert.That(homeTeam.Points, Is.EqualTo(1));
            Assert.That(awayTeam.Points, Is.EqualTo(1));
        }

        [Test]
        public void GetStandings_SortsTeamsByPoints()
        {
            // Arrange
            _manager.Teams.Add(new Team { Id = 1, Name = "Team1", Played = 10, Wins = 5, Draws = 3, Losses = 2, GoalsFor = 20, GoalsAgainst = 10 });
            _manager.Teams.Add(new Team { Id = 2, Name = "Team2", Played = 10, Wins = 8, Draws = 1, Losses = 1, GoalsFor = 25, GoalsAgainst = 8 });
            _manager.Teams.Add(new Team { Id = 3, Name = "Team3", Played = 10, Wins = 3, Draws = 4, Losses = 3, GoalsFor = 15, GoalsAgainst = 15 });

            // Act
            var standings = _manager.GetStandings();

            // Assert
            Assert.That(standings[0].Name, Is.EqualTo("Team2")); // 25 points
            Assert.That(standings[1].Name, Is.EqualTo("Team1")); // 18 points
            Assert.That(standings[2].Name, Is.EqualTo("Team3")); // 13 points
        }

        [Test]
        public void GetHeadToHead_ReturnsMatchesBetweenTwoTeams()
        {
            // Arrange
            var team1 = new Team { Id = 1, Name = "Team1" };
            var team2 = new Team { Id = 2, Name = "Team2" };
            var team3 = new Team { Id = 3, Name = "Team3" };

            _manager.Matches.Add(new Match { Id = 1, HomeTeam = team1, AwayTeam = team2, IsPlayed = true });
            _manager.Matches.Add(new Match { Id = 2, HomeTeam = team2, AwayTeam = team1, IsPlayed = true });
            _manager.Matches.Add(new Match { Id = 3, HomeTeam = team1, AwayTeam = team3, IsPlayed = true });

            // Act
            var headToHead = _manager.GetHeadToHead(team1, team2);

            // Assert
            Assert.That(headToHead.Count, Is.EqualTo(2));
        }

        [Test]
        public void SearchTeams_FindsTeamsByPredicate()
        {
            // Arrange
            _manager.Teams.Add(new Team { Id = 1, Name = "Team1", Wins = 10 });
            _manager.Teams.Add(new Team { Id = 2, Name = "Team2", Wins = 5 });
            _manager.Teams.Add(new Team { Id = 3, Name = "Team3", Wins = 15 });

            // Act
            var result = _manager.SearchTeams(t => t.Wins > 7);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void CreatePredictionCopy_CreatesIndependentCopy()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Team1", Wins = 5 };
            _manager.Teams.Add(team);

            // Act
            var copy = _manager.CreatePredictionCopy();
            copy.Teams[0].Wins = 10;

            // Assert
            Assert.That(_manager.Teams[0].Wins, Is.EqualTo(5)); // Оригинал не изменился
            Assert.That(copy.Teams[0].Wins, Is.EqualTo(10)); // Копия изменилась
        }
    }
}
