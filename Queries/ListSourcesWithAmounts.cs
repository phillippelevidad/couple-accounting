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
            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Total { get; set; }
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
                var sourcesWithAmounts = await conn.QueryAsync<ListSourcesWithAmountsResult.SourceItem>(sql, new { start = request.Start, end = request.End });
                return new ListSourcesWithAmountsResult(sourcesWithAmounts);
            }
        }

        private const string sql = @"
            SELECT PaymentSources.Id, PaymentSources.Name, IFNULL(SUM(Payments.Amount), 0) Total
            FROM PaymentSources
            LEFT JOIN Payments ON PaymentSources.Id = Payments.SourceId
                AND Payments.DateTime >= @start AND Payments.DateTime <= @end
            WHERE PaymentSources.IsDeleted = 0";
    }
}
