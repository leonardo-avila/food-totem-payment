using FoodTotem.Data.Core;
using FoodTotem.Payment.Domain.Models;

namespace FoodTotem.Payment.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Pay>
    {
        Task<Pay> GetPayment(string orderReference);
        Task<bool> SavePayment(Pay payment);
    }
}