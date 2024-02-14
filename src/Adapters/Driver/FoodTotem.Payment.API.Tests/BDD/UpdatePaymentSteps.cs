using FoodTotem.Payment.API.Controllers;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.OutputViewModels;
using FoodTotem.Payment.UseCase.Ports;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TechTalk.SpecFlow;

namespace FoodTotem.Payment.API.Tests.BDD
{
    [Binding]
    public class UpdatePaymentSteps
    {
        private readonly IPaymentUseCases _paymentUseCases = Substitute.For<IPaymentUseCases>();
        private PaymentViewModel _payment;
        private ActionResult<PaymentViewModel> _paymentResult;
        private readonly PaymentController _controller;
        private PaymentStatusViewModel _paymentStatus;

        public UpdatePaymentSteps()
        {
            _controller = new PaymentController(_paymentUseCases);
        }

        [Given(@"I have a payment with status Pending")]
        public void GivenIHaveAPaymentWithStatusPending()
        {
            _payment = new PaymentViewModel
            {
                Id = "1234",
                OrderReference = "1234",
                ExpirationDate = "2021-10-10",
                QRCode = "1234",
                Total = 10.0,
                Status = "Pending"
            };
        }

        [When(@"I update the payment with status Paid")]
        public async Task WhenIUpdateThePaymentWithStatusPaid()
        {
            _paymentStatus = new PaymentStatusViewModel
            {
                Id = "1234",
                IsApproved = true
            };
            _payment.Status = "Paid";
            _paymentUseCases.UpdatePaymentStatus(_paymentStatus).Returns(x => _payment);
            _paymentResult = await _controller.UpdatePaymentStatus(_paymentStatus);
        }

        [When(@"I update the payment with status Cancelled")]
        public async Task WhenIUpdateThePaymentWithStatusCancelled()
        {
            _paymentStatus = new PaymentStatusViewModel
            {
                Id = "1234",
                IsApproved = false
            };
            _payment.Status = "Cancelled";
            _paymentUseCases.UpdatePaymentStatus(_paymentStatus).Returns(x => _payment);
            _paymentResult = await _controller.UpdatePaymentStatus(_paymentStatus);
        }

        [Then(@"the payment status should be Paid")]
        public void ThenThePaymentStatusShouldBePaid()
        {
            Assert.IsInstanceOfType(_paymentResult.Result, typeof(OkObjectResult));
            if (_paymentResult.Result is OkObjectResult paymentResult)
            {
                if (paymentResult.Value is PaymentViewModel paymentViewModel)
                {
                    Assert.AreEqual("Paid", paymentViewModel.Status);
                }
            }
        }

        [Then(@"the payment status should be Cancelled")]
        public void ThenThePaymentStatusShouldBeCancelled()
        {
            Assert.IsInstanceOfType(_paymentResult.Result, typeof(OkObjectResult));
            if (_paymentResult.Result is OkObjectResult paymentResult)
            {
                if (paymentResult.Value is PaymentViewModel paymentViewModel)
                {
                    Assert.AreEqual("Cancelled", paymentViewModel.Status);
                }
            }
        }
    }
}