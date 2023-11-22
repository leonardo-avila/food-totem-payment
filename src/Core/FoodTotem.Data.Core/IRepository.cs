using System.Linq.Expressions;
using FoodTotem.Domain.Core;
using MongoDB.Bson;

namespace FoodTotem.Data.Core
{
    public interface IRepository<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> GetAll();
        Task<TDocument> Get(ObjectId id);
        Task<TDocument> Get(string id);
        Task<TDocument> Get(Expression<Func<TDocument, bool>> filterExpression);
        Task<bool> Create(TDocument entity);
        Task<bool> Update(TDocument entity);
        Task<bool> Delete(TDocument entity);
    }
}