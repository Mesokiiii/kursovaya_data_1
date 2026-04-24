using NUnit.Framework;
using System.Collections.Generic;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class LeagueManagerExtendedTests
    {
        private LeagueManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new LeagueManager();
        }

        [Test]
        public void GenerateMockData_CreatesTeamsAndMatches()
        {
            // Act
            _manager.GenerateMockData();

            // Assert
            Assert.That(_manager.Teams.Count, Is.GreaterThan(0));
            Assert.That(_manager.Matches.Count, Is.GreaterThan(0));
        }

        [Test]
        public void ProcessMatchResult_HomeWin_UpdatesStatistics()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Home" };
            var awayTeam = new Team { Id = 2, Name = "Away" };
            _manager.Teams.Add(homeTeam);
            _manager.Teams.Add(awayTeam);

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
            Assert.That(homeTeam.Wins, Is.EqualTo(1));
            Assert.That(homeTeam.Points, Is.EqualTo(3));
            Assert.That(awayTeam.Losses, Is.EqualTo(1));
            Assert.That(awayTeam.Points, Is.EqualTo(0));
        }

        [Test]
        public void ProcessMatchResult_Draw_UpdatesStatistics()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Home" };
            var awayTeam = new Team { Id = 2, Name = "Away" };
            _manager.Teams.Add(homeTeam);
            _manager.Teams.Add(awayTeam);

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
            Assert.That(homeTeam.Points, Is.EqualTo(1));
            Assert.That(awayTeam.Draws, Is.EqualTo(1));
            Assert.That(awayTeam.Points, Is.EqualTo(1));
        }

        [Test]
        public void ProcessMatchResult_AwayWin_UpdatesStatistics()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Home" };
            var awayTeam = new Team { Id = 2, Name = "Away" };
            _manager.Teams.Add(homeTeam);
            _manager.Teams.Add(awayTeam);

            var match = new Match
            {
                Id = 1,
                Round = 1,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeGoals = 0,
                AwayGoals = 2,
                IsPlayed = true
            };

            // Act
            _manager.ProcessMatchResult(match);

            // Assert
            Assert.That(homeTeam.Losses, Is.EqualTo(1));
            Assert.That(homeTeam.Points, Is.EqualTo(0));
            Assert.That(awayTeam.Wins, Is.EqualTo(1));
            Assert.That(awayTeam.Points, Is.EqualTo(3));
        }

        [Test]
        public void SearchTeamsByCriteria_WithMinWins_ReturnsCorrectTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(5);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                MinWins = 2
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.Wins, Is.GreaterThanOrEqualTo(2));
            }
        }

        [Test]
        public void SearchTeamsByCriteria_WithMinGoalDifference_ReturnsCorrectTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(5);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                MinGoalDifference = 0
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.GoalDifference, Is.GreaterThanOrEqualTo(0));
            }
        }

        [Test]
        public void SearchTeamsByCriteria_WithMinGoalsFor_ReturnsCorrectTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(5);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                MinGoalsFor = 3
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.GoalsFor, Is.GreaterThanOrEqualTo(3));
            }
        }

        [Test]
        public void SearchTeamsByCriteria_WithMaxGoalsFor_ReturnsCorrectTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(5);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                MaxGoalsFor = 10
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.GoalsFor, Is.LessThanOrEqualTo(10));
            }
        }

        [Test]
        public void SearchTeamsByCriteria_MultipleCriteria_ReturnsCorrectTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(5);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                MinPoints = 3,
                MinGoalsFor = 2
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.Points, Is.GreaterThanOrEqualTo(3));
                Assert.That(team.GoalsFor, Is.GreaterThanOrEqualTo(2));
            }
        }

        [Test]
        public void GetHeadToHead_NoMatches_ReturnsEmptyList()
        {
            // Arrange
            var team1 = new Team { Id = 1, Name = "Team 1" };
            var team2 = new Team { Id = 2, Name = "Team 2" };
            _manager.Teams.Add(team1);
            _manager.Teams.Add(team2);

            // Act
            var result = _manager.GetHeadToHead(team1, team2);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetStandings_EmptyLeague_ReturnsEmptyList()
        {
            // Act
            var standings = _manager.GetStandings();

            // Assert
            Assert.That(standings, Is.Not.Null);
            Assert.That(standings.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetTeamHistory_NonExistentTeam_ReturnsEmptyList()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);

            // Act
            var history = _manager.GetTeamHistory(999);

            // Assert
            Assert.That(history, Is.Not.Null);
            Assert.That(history.Count, Is.EqualTo(0));
        }

        [Test]
        public void ProcessAllMatches_WithUnplayedMatches_OnlyProcessesPlayed()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(2);
            var playedBefore = ManualAlgorithms.Count(_manager.Matches, m => m.IsPlayed);

            // Act
            _manager.ProcessAllMatches();

            // Assert
            var playedAfter = ManualAlgorithms.Count(_manager.Matches, m => m.IsPlayed);
            Assert.That(playedAfter, Is.EqualTo(playedBefore));
        }
    }
}
