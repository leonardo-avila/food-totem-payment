using FoodTotem.Domain.Core;
using FoodTotem.Payment.API.Controllers;
using FoodTotem.Payment.UseCase.OutputViewModels;
using FoodTotem.Payment.UseCase.Ports;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TechTalk.SpecFlow;

namespace FoodTotem.Payment.API.Tests.BDD;

[Binding]
public class GetPaymentSteps
{
    private readonly IPaymentUseCases _paymentUseCases = Substitute.For<IPaymentUseCases>();
    private PaymentViewModel _payment;
    private ActionResult<IEnumerable<PaymentViewModel>> _payments;
    private ActionResult<PaymentViewModel> _paymentResult;
    private readonly PaymentController _controller;

    public GetPaymentSteps()
    {
        _controller = new PaymentController(_paymentUseCases);
    }

    [Given(@"I have a payment")]
    public void GivenIHaveAPayment()
    {
        _payment = new PaymentViewModel
        {
            Id = "1234",
            OrderReference = "1234",
            ExpirationDate = "2021-10-10",
            QRCode = "1234",
            Total = 10.0
        };
    }

    [Given(@"there is a internal error")]
    public void WhenThereIsAInternalError()
    {
        _paymentUseCases.GetPayments().Returns<IEnumerable<PaymentViewModel>>(x => throw new Exception("An error occurred while retrieving payment"));
        _paymentUseCases.GetPaymentByOrderReference("1234").Returns<PaymentViewModel>(x => throw new Exception("An error occurred while retrieving payment"));
    }

    [When(@"I get the payments")]
    public async Task WhenIGetThePayments()
    {
        _payments = await _controller.Get();
    }

    [When(@"I get the payment by order reference")]
    public async Task WhenIGetThePaymentByOrderReference()
    {
        _paymentResult = await _controller.GetByOrderReference(_payment.OrderReference);
    }

    [When(@"I get the payment by unknown order reference")]
    public async Task WhenIGetThePaymentByUnknownOrderReference()
    {
        _paymentUseCases.GetPaymentByOrderReference("11").Returns<PaymentViewModel>(x => throw new DomainException("Payment not found"));
        _paymentResult = await _controller.GetByOrderReference("11");
    }

    [Then(@"I should see the payments")]
    public void ThenIShouldSeeThePayments()
    {
        Assert.IsInstanceOfType(_payments.Result, typeof(OkObjectResult));
    }

    [Then(@"I should receive a internal error for payments")]
    public void ThenIShouldReceiveAInternalErrorForPayments()
    {
        Assert.IsInstanceOfType(_payments.Result, typeof(ObjectResult));
        var result = _payments.Result as ObjectResult;
        Assert.AreEqual(500, result!.StatusCode);
    }

    [Then(@"I should receive a internal error for payment")]
    public void ThenIShouldReceiveAInternalErrorForPayment()
    {
        Assert.IsInstanceOfType(_paymentResult.Result, typeof(ObjectResult));
        var result = _paymentResult.Result as ObjectResult;
        Assert.AreEqual(500, result!.StatusCode);
    }

    [Then(@"I should see the payment")]
    public void ThenIShouldSeeThePayment()
    {
        Assert.IsInstanceOfType(_paymentResult.Result, typeof(OkObjectResult));
    }

    [Then(@"I should receive a domain error")]
    public void ThenIShouldReceiveADomainError()
    {
        Assert.IsInstanceOfType(_paymentResult.Result, typeof(BadRequestObjectResult));
    }
}