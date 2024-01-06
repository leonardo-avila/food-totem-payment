using FoodTotem.Domain.Core;
using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Ports;
using FoodTotem.Payment.Domain.Repositories;
using FoodTotem.Payment.Gateways.MercadoPago;
using FoodTotem.Payment.Gateways.MercadoPago.ViewModels;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.Ports;
using FoodTotem.Payment.UseCase.UseCases;
using NSubstitute;

namespace FoodTotem.Payment.UseCase.Tests;

[TestClass]
public class PaymentUseCasesTests
{
    private readonly IPaymentService _paymentService = Substitute.For<IPaymentService>();
    private readonly IPaymentRepository _paymentRepository = Substitute.For<IPaymentRepository>();
    private readonly IMercadoPagoPaymentService _mercadoPagoPaymentService = Substitute.For<IMercadoPagoPaymentService>();
    private IPaymentUseCases _paymentUseCases;

    [TestInitialize]
    public void Initialize()
    {
       _paymentUseCases = new PaymentUseCases(_paymentService, _paymentRepository, _mercadoPagoPaymentService);
    }

    [TestMethod, TestCategory("UseCase - Payment")]
    public async Task GetPayment_WithUnknownOrderReference_ShouldThrowException()
    {
        MockGetPaymentByOrderReference(_paymentRepository);
        var pay = await _paymentUseCases.GetPaymentByOrderReference("1234");

        Assert.IsNotNull(pay);
        Assert.AreEqual("1234", pay.OrderReference);
        Assert.AreEqual("2023-01-02", pay.ExpirationDate);
        Assert.AreEqual("QRCode", pay.QRCode);
        Assert.AreEqual(10.0, pay.Total);
    }

    [TestMethod, TestCategory("UseCase - Payment")]
    public async Task GetPayment_WithKnownOrderReference_ShouldReturnPayment()
    {
        MockGetPaymentByOrderReference(_paymentRepository);
        await Assert.ThrowsExceptionAsync<DomainException>(async () => await _paymentUseCases.GetPaymentByOrderReference("12"));
    }

    [TestMethod, TestCategory("UseCase - Payment")]
    public async Task GetPayments_ShouldReturnPayments()
    {
        MockGetAllPayments(_paymentRepository);
        var payments = await _paymentUseCases.GetPayments();

        Assert.IsNotNull(payments);
        Assert.AreEqual(2, payments.Count());
    }

    [TestMethod, TestCategory("UseCase - Payment")]
    public async Task CreatePayment_WithValidReference_ShouldSuccess() {
        MockMercadoPagoQRCode();
        MockValidatePayment();
        MockSavePayment();

        var order = new OrderViewModel() {
            OrderReference = "1234",
            Total = 10.0
        };

        var payment = await _paymentUseCases.CreatePayment(order);

        Assert.IsNotNull(payment);
        Assert.AreEqual("1234", payment.OrderReference);
        Assert.AreEqual("QRCode", payment.QRCode);
        Assert.AreEqual(10.0, payment.Total);
    }

    [TestMethod, TestCategory("UseCase - Payment")]
    public async Task CreatePayment_WithInvalidReference_ShouldThrowException() {
        MockMercadoPagoQRCode();
        MockSavePayment();

        var order = new OrderViewModel() {
            OrderReference = "1234",
            Total = 10.0
        };

        await Assert.ThrowsExceptionAsync<DomainException>(async () => await _paymentUseCases.CreatePayment(order));
    }

    [TestMethod, TestCategory("UseCase - Payment")]
    public async Task CreatePayment_WithSaveIssues_ShouldThrowException() {
        MockMercadoPagoQRCode();
        MockValidatePayment();

        var order = new OrderViewModel() {
            OrderReference = "1234",
            Total = 10.0
        };

        await Assert.ThrowsExceptionAsync<DomainException>(async () => await _paymentUseCases.CreatePayment(order));
    }

    private static void MockGetPaymentByOrderReference(IPaymentRepository paymentRepository)
    {
        var payment = new Pay("1234", "2023-01-02", "QRCode", 10.0);
        paymentRepository.GetPayment("1234").Returns(payment);
    }

    private static void MockGetAllPayments(IPaymentRepository paymentRepository)
    {
        var payments = new List<Pay>
        {
            new("1234", "2023-01-02", "QRCode", 10.0),
            new("123", "2023-01-02", "QRCode", 10.0)
        };
        paymentRepository.GetAll().Returns(payments);
    }

    private void MockMercadoPagoQRCode() {
        _mercadoPagoPaymentService.GetPaymentQRCode(null).ReturnsForAnyArgs(new QRCodeViewModel() {
            in_store_order_id = "1234",
            qr_data = "QRCode"
        });
    }

    private void MockValidatePayment() {
        _paymentService.IsValidPayment(null).ReturnsForAnyArgs(true);
    }

    private void MockSavePayment() {
        _paymentRepository.SavePayment(null).ReturnsForAnyArgs(true);
    }
}