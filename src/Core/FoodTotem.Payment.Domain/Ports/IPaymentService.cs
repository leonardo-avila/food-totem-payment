using FoodTotem.Payment.Domain.Models;

namespace FoodTotem.Payment.Domain.Ports
{
	public interface IPaymentService
	{
		bool IsValidPayment(Pay payment);
	}
}