using System.Linq.Expressions;
using Domain.Attributes;
using Domain.V1.Entities;
using Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public abstract class RepositoryBase<TDocument> : IRepositoryBase<TDocument> where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> collection;
        public RepositoryBase(AppDbContext.Connection dbContext)
        {
            IMongoDatabase database = dbContext();
            collection = database.GetCollection<TDocument>(RepositoryBase<TDocument>.GetCollectionName(typeof(TDocument)));
        }

        protected static string? GetCollectionName(Type documentType)
        {
            return (documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }

        protected abstract FilterDefinition<TDocument> GetAllFilterDefinition(string searchTerm);

        public virtual async Task<IQueryable<TDocument>> GetQueryableAsync()
        {
            return await Task.Run(() => collection.AsQueryable());
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual async Task<(IEnumerable<TDocument>, long TotalCount)> GetAllAsync(string searchTerm, string? sortColumn, string? sortDirection, int pageIndex, int pageSize)
        {
            var indexFilter = GetAllFilterDefinition(searchTerm);
            var totalRecords = await collection.CountDocumentsAsync(indexFilter);

            var data = collection.Find(indexFilter);

            if (!string.IsNullOrEmpty(sortColumn))
            {
                if (sortDirection == "ASC")
                    data = data.Sort(new SortDefinitionBuilder<TDocument>().Ascending(sortColumn));
                else
                    data = data.Sort(new SortDefinitionBuilder<TDocument>().Descending(sortColumn));
            }

            var result = await data.Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return (result, totalRecords);
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                return collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
            FilterDefinition<TDocument> filterExpression)
        {
            return (await collection.FindAsync(filterExpression)).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return (await collection.FindAsync(filterExpression)).ToEnumerable();
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => collection.InsertOneAsync(document));
        }

        public virtual async Task<TDocument> ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            return await collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task<TDocument> DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
            return await collection.FindOneAndDeleteAsync(filter);
        }
    }
}
