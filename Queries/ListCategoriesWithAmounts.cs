using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Queries
{
    public class ListCategoriesWithAmounts : IRequest<ListCategoriesWithAmountsResult>
    {
        public ListCategoriesWithAmounts(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
    }

    public class ListCategoriesWithAmountsResult
    {
        public ListCategoriesWithAmountsResult(IEnumerable<CategoryItem> categoriesWithAmounts)
        {
            Items = categoriesWithAmounts.ToList().AsReadOnly();
        }

        public ReadOnlyCollection<CategoryItem> Items { get; }

        public class CategoryItem
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public decimal Total { get; private set; }
        }
    }

    public class ListCategoriesWithAmountsHandler : IRequestHandler<ListCategoriesWithAmounts, ListCategoriesWithAmountsResult>
    {
        private readonly ConnectionFactory connectionFactory;

        public ListCategoriesWithAmountsHandler(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<ListCategoriesWithAmountsResult> Handle(ListCategoriesWithAmounts request, CancellationToken cancellationToken)
        {
            using (var conn = connectionFactory.Create())
            {
                var categoriesWithAmounts = await conn.QueryAsync<ListCategoriesWithAmountsResult.CategoryItem>(sql, new { request.Start, request.End });
                return new ListCategoriesWithAmountsResult(categoriesWithAmounts);
            }
        }

        private const string sql = @"
            SELECT Categories.Id, Categories.Name, SUM(Payments.Amount) Total
            FROM Categories
            INNER JOIN Payments ON Categories.Id = Payments.CategoryId
            WHERE Categories.IsDeleted = 0
                AND Payments.DateTime >= @start AND Payments.DateTime <= @end";
    }
}
