using NUnit.Framework;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class EnumsTests
    {
        [Test]
        public void MatchResult_HasCorrectValues()
        {
            Assert.That((int)MatchResult.Win, Is.EqualTo(0));
            Assert.That((int)MatchResult.Draw, Is.EqualTo(1));
            Assert.That((int)MatchResult.Loss, Is.EqualTo(2));
        }

        [Test]
        public void MatchResult_CanBeCompared()
        {
            var win = MatchResult.Win;
            var draw = MatchResult.Draw;
            var loss = MatchResult.Loss;

            Assert.That(win, Is.Not.EqualTo(draw));
            Assert.That(draw, Is.Not.EqualTo(loss));
            Assert.That(win, Is.Not.EqualTo(loss));
        }

        [Test]
        public void MatchStatus_HasCorrectValues()
        {
            Assert.That((int)MatchStatus.Scheduled, Is.EqualTo(0));
            Assert.That((int)MatchStatus.InProgress, Is.EqualTo(1));
            Assert.That((int)MatchStatus.Finished, Is.EqualTo(2));
            Assert.That((int)MatchStatus.Cancelled, Is.EqualTo(3));
        }

        [Test]
        public void SortType_HasCorrectValues()
        {
            Assert.That((int)SortType.Points, Is.EqualTo(0));
            Assert.That((int)SortType.GoalDifference, Is.EqualTo(1));
            Assert.That((int)SortType.GoalsFor, Is.EqualTo(2));
            Assert.That((int)SortType.TeamName, Is.EqualTo(3));
        }
    }
}
