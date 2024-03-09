using System.Diagnostics.CodeAnalysis;

namespace Domain.Attributes
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute(string collectionName) : Attribute
    {
        public string CollectionName { get; } = collectionName;
    }
}
