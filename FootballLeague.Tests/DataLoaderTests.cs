using NUnit.Framework;
using System;
using System.IO;
using FootballLeague;

namespace FootballLeague.Tests
{
    [TestFixture]
    public class DataLoaderTests
    {
        private string _testDataPath;

        [SetUp]
        public void Setup()
        {
            _testDataPath = Path.Combine(Path.GetTempPath(), "test_data.json");
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_testDataPath))
            {
                File.Delete(_testDataPath);
            }
        }

        [Test]
        public void LoadFromJson_ValidFile_LoadsDataSuccessfully()
        {
            // Arrange
            var manager = new LeagueManager();
            var jsonContent = @"{
                ""teams"": [
                    {
                        ""id"": 1,
                        ""name"": ""Test Team 1"",
                        ""played"": 0,
                        ""wins"": 0,
                        ""draws"": 0,
                        ""losses"": 0,
                        ""goalsFor"": 0,
                        ""goalsAgainst"": 0,
                        ""points"": 0
                    },
                    {
                        ""id"": 2,
                        ""name"": ""Test Team 2"",
                        ""played"": 0,
                        ""wins"": 0,
                        ""draws"": 0,
                        ""losses"": 0,
                        ""goalsFor"": 0,
                        ""goalsAgainst"": 0,
                        ""points"": 0
                    }
                ],
                ""matches"": [
                    {
                        ""id"": 1,
                        ""round"": 1,
                        ""homeTeamId"": 1,
                        ""awayTeamId"": 2,
                        ""homeGoals"": 2,
                        ""awayGoals"": 1,
                        ""isPlayed"": true
                    }
                ]
            }";
            File.WriteAllText(_testDataPath, jsonContent);

            // Act
            DataLoader.LoadFromJson(manager, _testDataPath);

            // Assert
            Assert.That(manager.Teams.Count, Is.EqualTo(2));
            Assert.That(manager.Matches.Count, Is.EqualTo(1));
            Assert.That(manager.Teams[0].Name, Is.EqualTo("Test Team 1"));
            Assert.That(manager.Teams[1].Name, Is.EqualTo("Test Team 2"));
        }

        [Test]
        public void LoadFromJson_NonExistentFile_ThrowsException()
        {
            // Arrange
            var manager = new LeagueManager();
            var nonExistentPath = "non_existent_file.json";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => DataLoader.LoadFromJson(manager, nonExistentPath));
        }

        [Test]
        public void LoadFromJson_InvalidJson_ThrowsException()
        {
            // Arrange
            var manager = new LeagueManager();
            File.WriteAllText(_testDataPath, "{ invalid json }");

            // Act & Assert
            Assert.Throws<Exception>(() => DataLoader.LoadFromJson(manager, _testDataPath));
        }
    }
}
