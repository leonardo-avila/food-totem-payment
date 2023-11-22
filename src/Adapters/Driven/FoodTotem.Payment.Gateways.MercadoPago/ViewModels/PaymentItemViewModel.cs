namespace FoodTotem.Payment.Gateways.MercadoPago.ViewModels
{
	public class PaymentItemViewModel
	{
		public string sku_number { get; set; }
		public string category { get; set; }
		public string title { get; set; }
		public int quantity { get; set; }
		public string unit_measure { get; set; }
		public double unit_price { get; set; }
		public double total_amount { get; set; }
	}
}