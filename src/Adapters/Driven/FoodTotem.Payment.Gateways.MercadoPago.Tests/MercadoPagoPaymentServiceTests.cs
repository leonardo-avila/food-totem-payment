using FoodTotem.Payment.Gateways.Http;
using FoodTotem.Payment.Gateways.MercadoPago.ViewModels;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace FoodTotem.Payment.Gateways.MercadoPago.Tests;

[TestClass]
public class MercadoPagoPaymentServiceTests
{
    public IHttpHandler _httpHandler = Substitute.For<IHttpHandler>();
    public IConfiguration _configuration = Substitute.For<IConfiguration>();
    private IMercadoPagoPaymentService _mercadoPagoPaymentService;

    [TestInitialize]
    public void Initialize()
    {
        _mercadoPagoPaymentService = new MercadoPagoPaymentService(_httpHandler, _configuration);
    }

    [TestMethod, TestCategory("Gateway - MercadoPago")]
    public async Task CreateQRCode_FromPayment_ShouldSuccess()
    {
        var paymentInformationViewModel = new PaymentInformationViewModel()
        {
            expiration_date = "2023-01-02T00:00:00.000-04:00",
            total_amount = 10.0,
            external_reference = "1234",
            title = "Food Totem Order",
            items = new List<PaymentItemViewModel>()
            {
                new()
                {
                    sku_number = "1234",
                    unit_measure = "unit",
                    unit_price = 10.0,
                    quantity = 1,
                    total_amount = 10.0,
                    title = "Food"
                }
            },
            description = "Food Totem Order 1234"
        };

        MockMercadoPagoQRCode(paymentInformationViewModel);
        var qrCode = await _mercadoPagoPaymentService.GetPaymentQRCode(paymentInformationViewModel);

        Assert.IsNotNull(qrCode);
        Assert.AreEqual("QRCode", qrCode.qr_data);
    }

    private void MockMercadoPagoQRCode(PaymentInformationViewModel paymentInformationViewModel)
    {
        _mercadoPagoPaymentService.GetPaymentQRCode(paymentInformationViewModel).ReturnsForAnyArgs(new QRCodeViewModel()
        {
            qr_data = "QRCode",
            in_store_order_id = "1234"
        });
    }
}