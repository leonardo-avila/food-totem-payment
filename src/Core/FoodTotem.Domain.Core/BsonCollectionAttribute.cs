using System.Diagnostics.CodeAnalysis;

namespace FoodTotem.Domain.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    [ExcludeFromCodeCoverage]
    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}