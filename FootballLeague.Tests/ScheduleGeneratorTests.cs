using NUnit.Framework;
using System;
using System.IO;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class ScheduleGeneratorTests
    {
        private string _testOutputPath;

        [SetUp]
        public void Setup()
        {
            _testOutputPath = Path.Combine(Path.GetTempPath(), "schedule_test.json");
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_testOutputPath))
            {
                File.Delete(_testOutputPath);
            }
        }

        [Test]
        public void GenerateAndSave_CreatesValidSchedule()
        {
            // Act
            ScheduleGenerator.GenerateAndSave(_testOutputPath, 4, 2);

            // Assert
            Assert.That(File.Exists(_testOutputPath), Is.True);
            
            var manager = new LeagueManager();
            DataLoader.LoadFromJson(manager, _testOutputPath);
            
            Assert.That(manager.Teams.Count, Is.EqualTo(4));
            Assert.That(manager.Matches.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GenerateAndSave_WithZeroTeams_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ScheduleGenerator.GenerateAndSave(_testOutputPath, 0, 0));
        }
    }
}
