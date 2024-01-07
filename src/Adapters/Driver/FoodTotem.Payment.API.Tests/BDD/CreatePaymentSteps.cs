using FoodTotem.Domain.Core;
using FoodTotem.Payment.API.Controllers;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.OutputViewModels;
using FoodTotem.Payment.UseCase.Ports;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TechTalk.SpecFlow;

namespace FoodTotem.Payment.API.Tests.BDD;

[Binding]
public class CreatePaymentSteps
{
    private readonly IPaymentUseCases _paymentUseCases = Substitute.For<IPaymentUseCases>();
    private OrderViewModel _order;
    private ActionResult<PaymentViewModel> _result;
    private readonly PaymentController _controller;

    public CreatePaymentSteps()
    {
        _controller = new PaymentController(_paymentUseCases);
    }

    [Given(@"I have an order")]
    public void GivenIHaveAnOrder()
    {
        _order = new OrderViewModel() {
            OrderReference = "1234",
            Total = 10.0
        };
    }

    [Given(@"I have an payment that will cause a domain exception")]
    public void GivenIHaveAnOrderThatWillCauseADomainException()
    {
        _paymentUseCases.CreatePayment(_order).Returns<PaymentViewModel>(x => throw new DomainException("Invalid payment"));
    }

    [When(@"I create a payment for the order")]
    public async Task WhenICreateAPaymentForTheOrder()
    {
        _result = await _controller.CreatePayment(_order);
    }

    [Then(@"the payment should be created successfully")]
    public void ThenThePaymentShouldBeCreatedSuccessfully()
    {
        Assert.IsInstanceOfType(_result.Result, typeof(OkObjectResult));
    }

    [Then(@"I should receive a bad request response")]
    public void ThenIShouldReceiveABadRequestResponse()
    {
        Assert.IsInstanceOfType(_result.Result, typeof(BadRequestObjectResult));
    }
}