using Xunit;
using VillageOfTesting;
using VillageOfTesting.Objects;
using VillageOfTesting.CompleteActions;
using VillageOfTesting.Interfaces;
using Moq;
using System.Collections.Generic;

public class UnitTest1
{
    [Fact]
    public void AddWorker_ShouldAddWorker_WhenOccupationIsValid()
    {
        // Arrange
        var village = new Village();
        string workerName = "Alice";
        string occupation = "farmer";

        // Act
        village.AddWorker(workerName, occupation);

        // Assert
        Assert.Single(village.Workers);
        Assert.Equal(workerName, village.Workers[0].Name);
        Assert.Equal(occupation, village.Workers[0].Occupation);
    }

    [Fact]
    public void AddWorker_ShouldNotAddWorker_WhenOccupationIsInvalid()
    {
        // Arrange
        var village = new Village();
        string workerName = "Bob";
        string invalidOccupation = "wizard";

        // Act
        village.AddWorker(workerName, invalidOccupation);

        // Assert
        Assert.Empty(village.Workers);
    }

    [Fact]
    public void AddProject_ShouldAddProject_WhenResourcesAreSufficient()
    {
        // Arrange
        var village = new Village
        {
            Wood = 10,
            Metal = 10
        };
        string projectName = "House";

        // Act
        village.AddProject(projectName);

        // Assert
        Assert.Single(village.Projects);
        Assert.Equal(projectName, village.Projects[0].Name);
    }

    [Fact]
    public void AddProject_ShouldNotAddProject_WhenResourcesAreInsufficient()
    {
        // Arrange
        var village = new Village
        {
            Wood = 1,
            Metal = 1
        };
        string projectName = "House";

        // Act
        village.AddProject(projectName);

        // Assert
        Assert.Empty(village.Projects);
    }

    [Fact]
    public void Day_ShouldDecreaseFoodAndFeedWorkers()
    {
        // Arrange
        var village = new Village
        {
            Food = 5
        };
        village.AddWorker("Alice", "farmer");

        // Act
        village.Day();

        // Assert
        Assert.Equal(4, village.Food);
        Assert.Equal(0, village.Workers[0].DaysHungry);
    }

    [Fact]
    public void Day_ShouldNotFeedWorkers_WhenNoFood()
    {
        // Arrange
        var village = new Village
        {
            Food = 0
        };
        village.AddWorker("Bob", "farmer");

        // Act
        village.Day();

        // Assert
        Assert.Equal(0, village.Food);
        Assert.Equal(1, village.Workers[0].DaysHungry);
    }

    [Fact]
    public void Save_ShouldCallDatabaseConnectionSaveVillage()
    {
        // Arrange
        var village = new Village();
        var mockDb = new Mock<DatabaseConnection>();
        var villageInput = new VillageInput(village, mockDb.Object);
        mockDb.Setup(db => db.SaveVillage(village, "TestVillage")).Returns(true);

        // Act
        villageInput.Save();

        // Assert
        mockDb.Verify(db => db.SaveVillage(village, "TestVillage"), Times.Once);
    }

    [Fact]
    public void Load_ShouldLoadVillageFromDatabase()
    {
        // Arrange
        var mockDb = new Mock<DatabaseConnection>();
        var expectedVillage = new Village();
        mockDb.Setup(db => db.LoadVillage("TestVillage")).Returns(expectedVillage);
        var villageInput = new VillageInput(new Village(), mockDb.Object);

        // Act
        villageInput.Load();

        // Assert
        mockDb.Verify(db => db.LoadVillage("TestVillage"), Times.Once);
        Assert.NotNull(villageInput);
    }
}
