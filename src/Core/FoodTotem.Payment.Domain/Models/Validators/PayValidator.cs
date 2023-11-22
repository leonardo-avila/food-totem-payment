using FluentValidation;

namespace FoodTotem.Payment.Domain.Models.Validators
{
	public class PayValidator : AbstractValidator<Pay>
    {
		public PayValidator()
		{
			RuleFor(p => p.OrderReference).NotNull();
			RuleFor(p => p.ExpirationDate).NotNull();
			RuleFor(p => p.CreatedAt).NotNull();
			RuleFor(p => p.QRCode).NotNull();
		}
	}
}