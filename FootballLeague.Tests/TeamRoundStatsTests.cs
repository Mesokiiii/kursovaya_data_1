using NUnit.Framework;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class TeamRoundStatsTests
    {
        [Test]
        public void TeamRoundStats_Properties_CanBeSetAndGet()
        {
            // Arrange & Act
            var stats = new TeamRoundStats
            {
                TeamId = 1,
                TeamName = "Test Team",
                Round = 5,
                Position = 3,
                Points = 12
            };

            // Assert
            Assert.That(stats.TeamId, Is.EqualTo(1));
            Assert.That(stats.TeamName, Is.EqualTo("Test Team"));
            Assert.That(stats.Round, Is.EqualTo(5));
            Assert.That(stats.Position, Is.EqualTo(3));
            Assert.That(stats.Points, Is.EqualTo(12));
        }

        [Test]
        public void TeamRoundStats_DefaultValues_AreZero()
        {
            // Arrange & Act
            var stats = new TeamRoundStats();

            // Assert
            Assert.That(stats.Round, Is.EqualTo(0));
            Assert.That(stats.Position, Is.EqualTo(0));
            Assert.That(stats.Points, Is.EqualTo(0));
        }
    }
}
