using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Models.Validators;

namespace FoodTotem.Payment.Domain.Tests;

[TestClass]
public class PayValidatorTests
{
    [TestMethod, TestCategory("Domain - Validators - Pay")]
    public void Pay_WithIncorrectAttributes_ShouldBeInvalid()
    {
        // Arrange
        var pay = new Pay("123456789", "2021-10-10", null, 10.0);

        // Act
        var validator = new PayValidator();
        var result = validator.Validate(pay);

        // Assert
        Assert.IsFalse(result.IsValid);
        
    }

    [TestMethod, TestCategory("Domain - Validators - Pay")]
    public void Pay_WithCorrectAttributes_ShouldBeValid()
    {
        // Arrange
        var pay = new Pay("123456789", "2021-10-10", "QRCode", 10.0);

        // Act
        var validator = new PayValidator();
        var result = validator.Validate(pay);

        // Assert
        Assert.IsTrue(result.IsValid);
    }
}