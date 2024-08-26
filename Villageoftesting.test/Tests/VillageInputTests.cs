using Xunit;
using Moq;
using VillageOfTesting;
using System.Collections.Generic;

public class VillageInputTests
{
    [Fact]
    public void Save_ValidVillage_SavesSuccessfully()
    {
        // Arrange
        var village = new Village();
        var mockDatabase = new Mock<DatabaseConnection>();
        mockDatabase.Setup(db => db.SaveVillage(village, It.IsAny<string>())).Returns(true);
        var villageInput = new VillageInput(village, mockDatabase.Object);

        // Act
        villageInput.Save();

        // Assert
        mockDatabase.Verify(db => db.SaveVillage(village, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Load_ValidVillage_LoadsSuccessfully()
    {
        // Arrange
        var village = new Village();
        var loadedVillage = new Village() { Food = 50 }; // Simulated loaded data
        var mockDatabase = new Mock<DatabaseConnection>();
        mockDatabase.Setup(db => db.GetTownNames()).Returns(new List<string> { "TestVillage" });
        mockDatabase.Setup(db => db.LoadVillage("TestVillage")).Returns(loadedVillage);

        var villageInput = new VillageInput(village, mockDatabase.Object);

        // Act
        villageInput.Load();

        // Assert
        Assert.Equal(50, village.Food);
    }
}
