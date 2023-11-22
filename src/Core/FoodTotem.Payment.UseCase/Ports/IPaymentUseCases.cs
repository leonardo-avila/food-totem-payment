using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.OutputViewModels;

namespace FoodTotem.Payment.UseCase.Ports
{
    public interface IPaymentUseCases
    {
        Task<PaymentViewModel> CreatePayment(OrderViewModel order);
        Task<PaymentViewModel> GetPayment(string id);
        Task<PaymentViewModel> GetPaymentByOrderReference(string orderReference);
        Task<IEnumerable<PaymentViewModel>> GetPayments();
    }
}