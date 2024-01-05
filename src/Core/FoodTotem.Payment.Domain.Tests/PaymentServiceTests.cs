using FoodTotem.Domain.Core;
using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Models.Validators;
using FoodTotem.Payment.Domain.Services;

namespace FoodTotem.Payment.Domain.Tests;

[TestClass]
public class PaymentServiceTests
{
    [TestMethod, TestCategory("Domain - Services - Payment")]
    public void Pay_WithIncorrectAttributes_ShouldBeInvalid()
    {
        // Arrange
        var pay = new Pay("123456789", "2021-10-10", null, 10.0);

        // Act
        var validator = new PayValidator();
        var paymentService = new PaymentService(validator);
        
        // Assert
        Assert.ThrowsException<DomainException>(() => paymentService.IsValidPayment(pay));
    }

    [TestMethod, TestCategory("Domain - Services - Payment")]
    public void Pay_WithCorrectAttributes_ShouldBeValid()
    {
        // Arrange
        var pay = new Pay("123456789", "2021-10-10", "QRCode", 10.0);

        // Act
        var validator = new PayValidator();
        var paymentService = new PaymentService(validator);
        var result = paymentService.IsValidPayment(pay);

        // Assert
        Assert.IsTrue(result);
    }
}