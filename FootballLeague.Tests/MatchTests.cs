using NUnit.Framework;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class MatchTests
    {
        [Test]
        public void Match_Properties_CanBeSetAndGet()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Home Team" };
            var awayTeam = new Team { Id = 2, Name = "Away Team" };

            // Act
            var match = new Match
            {
                Id = 1,
                Round = 5,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeGoals = 3,
                AwayGoals = 2,
                IsPlayed = true
            };

            // Assert
            Assert.That(match.Id, Is.EqualTo(1));
            Assert.That(match.Round, Is.EqualTo(5));
            Assert.That(match.HomeTeam.Name, Is.EqualTo("Home Team"));
            Assert.That(match.AwayTeam.Name, Is.EqualTo("Away Team"));
            Assert.That(match.HomeGoals, Is.EqualTo(3));
            Assert.That(match.AwayGoals, Is.EqualTo(2));
            Assert.That(match.IsPlayed, Is.True);
        }

        [Test]
        public void Match_DefaultValues()
        {
            // Act
            var match = new Match();

            // Assert
            Assert.That(match.Id, Is.EqualTo(0));
            Assert.That(match.Round, Is.EqualTo(0));
            Assert.That(match.HomeGoals, Is.EqualTo(0));
            Assert.That(match.AwayGoals, Is.EqualTo(0));
            Assert.That(match.IsPlayed, Is.False);
        }

        [Test]
        public void Match_UnplayedMatch_HasZeroGoals()
        {
            // Arrange
            var homeTeam = new Team { Id = 1, Name = "Team A" };
            var awayTeam = new Team { Id = 2, Name = "Team B" };

            // Act
            var match = new Match
            {
                Id = 1,
                Round = 1,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                IsPlayed = false
            };

            // Assert
            Assert.That(match.HomeGoals, Is.EqualTo(0));
            Assert.That(match.AwayGoals, Is.EqualTo(0));
        }
    }
}
