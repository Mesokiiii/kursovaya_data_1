using NUnit.Framework;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ConstantsTests
    {
        [Test]
        public void Constants_PointsForWin_IsThree()
        {
            Assert.That(Constants.POINTS_FOR_WIN, Is.EqualTo(3));
        }

        [Test]
        public void Constants_PointsForDraw_IsOne()
        {
            Assert.That(Constants.POINTS_FOR_DRAW, Is.EqualTo(1));
        }

        [Test]
        public void Constants_PointsForLoss_IsZero()
        {
            Assert.That(Constants.POINTS_FOR_LOSS, Is.EqualTo(0));
        }
    }
}
