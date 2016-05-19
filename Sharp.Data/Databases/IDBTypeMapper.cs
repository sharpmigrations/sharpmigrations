using System;
using System.Data;

namespace Sharp.Data {
    public interface IDbTypeMapper {
        DbType GetDbType(Type type);
    }

}