using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Ports;
using FoodTotem.Domain.Core;
using FluentValidation;

namespace FoodTotem.Payment.Domain.Services
{
	public class PaymentService : IPaymentService
	{
        private readonly IValidator<Pay> _payValidator;

		public PaymentService(IValidator<Pay> payValidator)
		{
            _payValidator = payValidator;
        }

		public bool IsValidPayment(Pay payment)
		{
            var validationResult = _payValidator.Validate(payment);
            if (!validationResult.IsValid) throw new DomainException(validationResult.ToString());
            
            return true;
        }
    }
}