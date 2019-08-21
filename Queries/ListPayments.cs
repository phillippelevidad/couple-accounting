using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Queries
{
    public class ListPayments : IRequest<ListPaymentsResult>
    {
        public ListPayments(DateTime start, DateTime end, Paging paging)
        {
            Start = start;
            End = end;
            Paging = paging;
        }

        public DateTime Start { get; }
        public DateTime End { get; }
        public Paging Paging { get; }
    }

    public class ListPaymentsResult
    {
        public ListPaymentsResult(IEnumerable<PaymentDto> payments)
        {
            Payments = payments.ToList().AsReadOnly();
        }

        public IReadOnlyCollection<PaymentDto> Payments { get; }

        public class PaymentDto
        {
            public Guid Id { get; private set; }
            public Guid SourceId { get; private set; }
            public Guid CategoryId { get; private set; }
            public DateTimeOffset DateTime { get; private set; }
            public decimal Amount { get; private set; }
            public string SourceName { get; private set; }
            public string CategoryName { get; private set; }
        }
    }

    public class ListPaymentsHandler : IRequestHandler<ListPayments, ListPaymentsResult>
    {
        private readonly ConnectionFactory connectionFactory;

        public ListPaymentsHandler(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<ListPaymentsResult> Handle(ListPayments request, CancellationToken cancellationToken)
        {
            using (var conn = connectionFactory.Create())
            {
                var parameters = new { start = request.Start, end = request.End, startIndex = request.Paging.StartIndex, pageSize = request.Paging.PageSize };
                var payments = await conn.QueryAsync<ListPaymentsResult.PaymentDto>(sql, parameters);
                return new ListPaymentsResult(payments);
            }
        }

        private const string sql = @"
            SELECT 
                Payments.Id, Payments.DateTime, Payments.Amount,
                PaymentSources.Id SourceId, PaymentSources.Name SourceName,
                Categories.Id CategoryId, Categories.Name CategoryName
            FROM Payments
            INNER JOIN PaymentSources ON Payments.SourceId = PaymentSources.Id
            INNER JOIN Categories ON Payments.CategoryId = Categories.Id
            WHERE Payments.DateTime >= @start AND Payments.DateTime <= @end
            LIMIT @pageSize OFFSET @startIndex";
    }
}
