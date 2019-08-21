using Dapper;
using System;
using System.Data;

namespace Queries
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value)
        {
            var valueAsBytes = (byte[])value;
            return new Guid(valueAsBytes);
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, Guid value)
        {
            var guidAsBytes = value.ToByteArray();
            parameter.Value = guidAsBytes;
        }
    }

    public class DateTimeOffsetTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
        {
            return DateTimeOffset.Parse((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            var serialized = value.ToString("O");
            parameter.Value = serialized;
        }
    }
}
