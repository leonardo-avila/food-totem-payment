using System.Diagnostics.CodeAnalysis;

namespace FoodTotem.Domain.Core;
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }
    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}