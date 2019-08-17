using CSharpFunctionalExtensions;
using Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public interface ICategoryRepository
    {
        Task<Result> AddAsync(Category category, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Category> FindAsync(Guid id, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(Category category, CancellationToken cancellationToken);
    }
}
