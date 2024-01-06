namespace FoodTotem.Payment.Gateways.MercadoPago.ViewModels
{
	[ExcludeFromCodeCoverage]
	public class PaymentInformationViewModel
	{
		public string external_reference { get; set; }
		public double total_amount { get; set; }
		public IEnumerable<PaymentItemViewModel> items { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string expiration_date { get; set; }
	}
}