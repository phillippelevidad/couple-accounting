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
    public class ListSourcesWithAmounts : IRequest<ListSourcesWithAmountsResult>
    {
        public ListSourcesWithAmounts(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
    }

    public class ListSourcesWithAmountsResult
    {
        public ListSourcesWithAmountsResult(IEnumerable<SourceItem> sourcesWithAmounts)
        {
            Items = sourcesWithAmounts.ToList().AsReadOnly();
        }

        public ReadOnlyCollection<SourceItem> Items { get; }

        public class SourceItem
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public decimal Total { get; private set; }
        }
    }

    public class ListSourcesWithAmountsHandler : IRequestHandler<ListSourcesWithAmounts, ListSourcesWithAmountsResult>
    {
        private readonly ConnectionFactory connectionFactory;

        public ListSourcesWithAmountsHandler(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<ListSourcesWithAmountsResult> Handle(ListSourcesWithAmounts request, CancellationToken cancellationToken)
        {
            using (var conn = connectionFactory.Create())
            {
                var sourcesWithAmounts = await conn.QueryAsync<ListSourcesWithAmountsResult.SourceItem>(sql, new { request.Start, request.End });
                return new ListSourcesWithAmountsResult(sourcesWithAmounts);
            }
        }

        private const string sql = @"
            SELECT PaymentSources.Id, PaymentSources.Name, SUM(Payments.Amount) Total
            FROM PaymentSources
            INNER JOIN Payments ON PaymentSources.Id = Payments.SourceId
            WHERE PaymentSources.IsDeleted = 0
                AND Payments.DateTime >= @start AND Payments.DateTime <= @end";
    }
}
