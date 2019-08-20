using Dapper;
using System;

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
}
