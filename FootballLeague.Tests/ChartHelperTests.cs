using NUnit.Framework;
using System.Collections.Generic;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ChartHelperTests
    {
        [Test]
        public void CreatePointsChart_ValidData_ReturnsPlotModel()
        {
            // Arrange
            var history = new List<TeamRoundStats>
            {
                new TeamRoundStats { Round = 1, Position = 1, Points = 3 },
                new TeamRoundStats { Round = 2, Position = 1, Points = 6 },
                new TeamRoundStats { Round = 3, Position = 2, Points = 7 }
            };

            // Act
            var chart = ChartHelper.CreatePointsChart(history, "Test Team");

            // Assert
            Assert.That(chart, Is.Not.Null);
            Assert.That(chart.Title, Does.Contain("Test Team"));
        }

        [Test]
        public void CreatePositionChart_ValidData_ReturnsPlotModel()
        {
            // Arrange
            var histories = new Dictionary<string, List<TeamRoundStats>>
            {
                {
                    "Team A",
                    new List<TeamRoundStats>
                    {
                        new TeamRoundStats { Round = 1, Position = 1, Points = 3 },
                        new TeamRoundStats { Round = 2, Position = 2, Points = 4 }
                    }
                },
                {
                    "Team B",
                    new List<TeamRoundStats>
                    {
                        new TeamRoundStats { Round = 1, Position = 2, Points = 1 },
                        new TeamRoundStats { Round = 2, Position = 1, Points = 4 }
                    }
                }
            };

            // Act
            var chart = ChartHelper.CreatePositionChart(histories);

            // Assert
            Assert.That(chart, Is.Not.Null);
        }

        [Test]
        public void CreatePointsChart_EmptyHistory_ReturnsPlotModel()
        {
            // Arrange
            var history = new List<TeamRoundStats>();

            // Act
            var chart = ChartHelper.CreatePointsChart(history, "Empty Team");

            // Assert
            Assert.That(chart, Is.Not.Null);
        }
    }
}
