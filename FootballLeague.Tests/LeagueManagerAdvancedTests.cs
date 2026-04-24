using NUnit.Framework;
using System.Linq;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class LeagueManagerAdvancedTests
    {
        private LeagueManager _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new LeagueManager();
        }

        [Test]
        public void GenerateFullSchedule_CreatesCorrectNumberOfMatches()
        {
            // Act
            _manager.GenerateFullSchedule(4);

            // Assert
            Assert.That(_manager.Teams.Count, Is.EqualTo(4));
            // 4 команды играют друг с другом дважды (дома и в гостях)
            // Количество матчей = n * (n-1) = 4 * 3 = 12
            Assert.That(_manager.Matches.Count, Is.EqualTo(12));
        }

        [Test]
        public void SimulateMatches_UpdatesTeamStatistics()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);

            // Act
            _manager.SimulateMatches(2);

            // Assert
            var playedMatches = ManualAlgorithms.Count(_manager.Matches, m => m.IsPlayed);
            Assert.That(playedMatches, Is.GreaterThan(0));
            
            var teamsWithGames = ManualAlgorithms.Count(_manager.Teams, t => t.Played > 0);
            Assert.That(teamsWithGames, Is.GreaterThan(0));
        }

        [Test]
        public void GetTeamHistory_ReturnsHistoryForAllRounds()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(3);
            var team = _manager.Teams[0];

            // Act
            var history = _manager.GetTeamHistory(team.Id);

            // Assert
            Assert.That(history, Is.Not.Null);
            Assert.That(history.Count, Is.GreaterThan(0));
        }

        [Test]
        public void CreatePredictionCopy_CreatesIndependentCopy()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(2);

            // Act
            var prediction = _manager.CreatePredictionCopy();

            // Assert
            Assert.That(prediction, Is.Not.Null);
            Assert.That(prediction.Teams.Count, Is.EqualTo(_manager.Teams.Count));
            Assert.That(prediction.Matches.Count, Is.EqualTo(_manager.Matches.Count));
        }

        [Test]
        public void ProcessAllMatches_UpdatesAllTeamStatistics()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            for (int i = 0; i < _manager.Matches.Count; i++)
            {
                var match = _manager.Matches[i];
                match.HomeGoals = 2;
                match.AwayGoals = 1;
                match.IsPlayed = true;
            }

            // Act
            _manager.ProcessAllMatches();

            // Assert
            bool allTeamsPlayed = true;
            for (int i = 0; i < _manager.Teams.Count; i++)
            {
                if (_manager.Teams[i].Played == 0)
                {
                    allTeamsPlayed = false;
                    break;
                }
            }
            Assert.That(allTeamsPlayed, Is.True);
        }

        [Test]
        public void GetHeadToHead_ReturnsOnlyMatchesBetweenTwoTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            var team1 = _manager.Teams[0];
            var team2 = _manager.Teams[1];

            // Act
            var headToHead = _manager.GetHeadToHead(team1, team2);

            // Assert
            Assert.That(headToHead, Is.Not.Null);
            foreach (var match in headToHead)
            {
                bool isCorrectMatch = (match.HomeTeam.Id == team1.Id && match.AwayTeam.Id == team2.Id) ||
                                     (match.HomeTeam.Id == team2.Id && match.AwayTeam.Id == team1.Id);
                Assert.That(isCorrectMatch, Is.True);
            }
        }

        [Test]
        public void SearchTeamsByCriteria_WithMinPoints_ReturnsCorrectTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(5);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                MinPoints = 6
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.Points, Is.GreaterThanOrEqualTo(6));
            }
        }

        [Test]
        public void SearchTeamsByCriteria_WithNameContains_ReturnsMatchingTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            
            var criteria = new LeagueManager.SearchCriteria
            {
                NameContains = "Спартак"
            };

            // Act
            var results = _manager.SearchTeamsByCriteria(criteria);

            // Assert
            Assert.That(results, Is.Not.Null);
            foreach (var team in results)
            {
                Assert.That(team.Name.ToLower(), Does.Contain("спартак".ToLower()));
            }
        }

        [Test]
        public void GetStandings_ReturnsSortedTeams()
        {
            // Arrange
            _manager.GenerateFullSchedule(4);
            _manager.SimulateMatches(3);

            // Act
            var standings = _manager.GetStandings();

            // Assert
            Assert.That(standings, Is.Not.Null);
            Assert.That(standings.Count, Is.EqualTo(4));
            
            // Проверяем, что команды отсортированы по очкам
            for (int i = 0; i < standings.Count - 1; i++)
            {
                Assert.That(standings[i].Points, Is.GreaterThanOrEqualTo(standings[i + 1].Points));
            }
        }
    }
}
