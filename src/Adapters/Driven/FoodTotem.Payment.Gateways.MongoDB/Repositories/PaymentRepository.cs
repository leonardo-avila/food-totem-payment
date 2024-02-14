using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Repositories;
using FoodTotem.Payment.Gateways.MongoDB.Setup;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FoodTotem.Payment.Gateways.MongoDB.Repositories
{
    public class PaymentRepository : RepositoryBase<Pay>, IPaymentRepository
    {
        private readonly IMongoCollection<Pay> _collection;
        public PaymentRepository(IConfiguration configuration) : base (configuration)
        {
            var settings = configuration.GetSection("PaymentDatabaseSettings").Get<PaymentDatabaseSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _collection = database.GetCollection<Pay>(GetCollectionName(typeof(Pay)));
        }

        public async Task<Pay> GetPayment(string orderReference)
        {
            var filter = Builders<Pay>.Filter.Eq("OrderReference", orderReference);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> SavePayment(Pay payment)
        {
            try 
            {
                await _collection.InsertOneAsync(payment);
                return true;
            } catch
            {
                return false;
            }
        }
    }
}