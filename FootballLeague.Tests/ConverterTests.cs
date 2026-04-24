using NUnit.Framework;
using System.Globalization;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ConverterTests
    {
        [Test]
        public void GoalDifferenceConverter_PositiveValue_ReturnsFormattedString()
        {
            // Arrange
            var converter = new GoalDifferenceConverter();

            // Act
            var result = converter.Convert(5, typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo("+5"));
        }

        [Test]
        public void GoalDifferenceConverter_NegativeValue_ReturnsFormattedString()
        {
            // Arrange
            var converter = new GoalDifferenceConverter();

            // Act
            var result = converter.Convert(-3, typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo("-3"));
        }

        [Test]
        public void GoalDifferenceConverter_ZeroValue_ReturnsZero()
        {
            // Arrange
            var converter = new GoalDifferenceConverter();

            // Act
            var result = converter.Convert(0, typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        [Test]
        public void PositionConverter_ValidPosition_ReturnsString()
        {
            // Arrange
            var converter = new PositionConverter();

            // Act
            var result = converter.Convert(1, typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo("1 место"));
        }

        [Test]
        public void ScoreConverter_ValidMatch_ReturnsFormattedScore()
        {
            // Arrange
            var converter = new ScoreConverter();
            var match = new Match
            {
                HomeGoals = 2,
                AwayGoals = 1,
                IsPlayed = true
            };

            // Act
            var result = converter.Convert(match, typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo("2 : 1"));
        }

        [Test]
        public void ScoreConverter_UnplayedMatch_ReturnsVersus()
        {
            // Arrange
            var converter = new ScoreConverter();
            var match = new Match
            {
                IsPlayed = false
            };

            // Act
            var result = converter.Convert(match, typeof(string), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.That(result, Is.EqualTo("vs"));
        }
    }
}
