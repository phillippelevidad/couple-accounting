using Application;
using CSharpFunctionalExtensions;
using Domain;
using Persistence.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AccountingContext context;

        public CategoryRepository(AccountingContext context)
        {
            this.context = context;
        }

        public async Task<Result> AddAsync(Category category, CancellationToken cancellationToken)
        {
            var model = DbCategory.From(category);
            context.Categories.Add(model);
            return await context.SaveChangesWithResultAsync(cancellationToken);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await context.Categories.FindAsync(new object[] { id }, cancellationToken);
            model.IsDeleted = true;
            return await context.SaveChangesWithResultAsync(cancellationToken);
        }

        public async Task<Category> FindAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await context.Categories.FindAsync(new object[] { id }, cancellationToken);
            return model.ToDomainModel();
        }

        public async Task<Result> UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            var model = await context.Categories.FindAsync(new object[] { category.Id }, cancellationToken);
            model.UpdateFrom(category);
            return await context.SaveChangesWithResultAsync(cancellationToken);
        }
    }
}
