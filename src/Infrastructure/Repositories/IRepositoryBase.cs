using System.Linq.Expressions;
using Domain.V1.Entities;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public interface IRepositoryBase<TDocument> where TDocument : IDocument
    {
        Task<(IEnumerable<TDocument>, long TotalCount)> GetAllAsync(string searchTerm, string? sortColumn, string? sortDirection, int pageIndex, int pageSize);
        Task<TDocument> FindByIdAsync(string id);
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<IEnumerable<TDocument>> FilterByAsync(FilterDefinition<TDocument> filterExpression);
        Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task InsertOneAsync(TDocument document);
        Task<TDocument> ReplaceOneAsync(TDocument document);
        Task<TDocument> DeleteByIdAsync(string id);
    }
}