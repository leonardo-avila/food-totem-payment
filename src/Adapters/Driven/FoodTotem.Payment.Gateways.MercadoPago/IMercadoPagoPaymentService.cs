using FoodTotem.Payment.Gateways.MercadoPago.ViewModels;

namespace FoodTotem.Payment.Gateways.MercadoPago
{
	public interface IMercadoPagoPaymentService
	{
		Task<QRCodeViewModel> GetPaymentQRCode(PaymentInformationViewModel paymentInformationViewModel);
	}
}