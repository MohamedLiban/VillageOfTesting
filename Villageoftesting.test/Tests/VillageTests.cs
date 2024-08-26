using Xunit;
using VillageOfTesting;
using System.Collections.Generic;
using VillageOfTesting.Objects;

public class VillageTests
{
    [Fact]
    public void AddWorker_ValidWorker_AddsWorkerSuccessfully()
    {
        // Arrange
        var village = new Village();
        string workerName = "John";
        string occupation = "farmer";

        // Act
        village.AddWorker(workerName, occupation);

        // Assert
        Assert.Single(village.Workers);
        Assert.Equal(workerName, village.Workers[0].Name);
    }

    [Fact]
    public void AddWorker_InvalidOccupation_DoesNotAddWorker()
    {
        // Arrange
        var village = new Village();
        string workerName = "John";
        string occupation = "invalidJob";

        // Act
        village.AddWorker(workerName, occupation);

        // Assert
        Assert.Empty(village.Workers);
    }

    [Fact]
    public void AddProject_ValidProject_AddsProjectSuccessfully()
    {
        // Arrange
        var village = new Village();
        village.Wood = 10;
        village.Metal = 10;

        // Act
        village.AddProject("House");

        // Assert
        Assert.Single(village.Projects);
        Assert.Equal("House", village.Projects[0].Name);
    }

    [Fact]
    public void AddProject_NotEnoughResources_DoesNotAddProject()
    {
        // Arrange
        var village = new Village();
        village.Wood = 2;
        village.Metal = 2;

        // Act
        village.AddProject("House");

        // Assert
        Assert.Empty(village.Projects);
    }

    [Fact]
    public void Day_AllWorkersFed_WorkersPerformWork()
    {
        // Arrange
        var village = new Village();
        village.AddWorker("John", "farmer");
        village.Food = 10;

        // Act
        village.Day();

        // Assert
        Assert.Equal(1, village.DaysGone);
        Assert.Equal(9, village.Food);
    }

    [Fact]
    public void Day_NoFood_WorkersStarve()
    {
        // Arrange
        var village = new Village();
        village.AddWorker("John", "farmer");
        village.Food = 0;

        // Act
        village.Day();

        // Assert
        Assert.Equal(1, village.Workers[0].DaysHungry);
    }

    [Fact]
    public void Day_AllWorkersDie_GameOver()
    {
        // Arrange
        var village = new Village();
        village.AddWorker("John", "farmer");
        village.Food = 0;

        // Simulate starvation
        for (int i = 0; i < Worker.daysUntilStarvation + 1; i++)
        {
            village.Day();
        }

        // Assert
        Assert.True(village.GameOver);
    }
}
