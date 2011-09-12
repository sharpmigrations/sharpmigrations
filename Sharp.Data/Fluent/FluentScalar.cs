using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Fluent {
    public class FluentScalar<T> {

        //private IDataClient _client;
        //private string _table;
        //private FluentWhere _fluentWhere;

        //public FluentScalar(IDataClient client, string table, FluentWhere fluentWhere)
        //    : this(client, table) {
        //    _fluentWhere = fluentWhere;
        //}

        //public FluentScalar(IDataClient client, string table) {
        //    _client = client;
        //    _table = table;
        //}

        //public T Column(string column) {
        //    string sql = _client.Dialect.GetSelectSql(_table, new string[] { column } );

        //    if (_fluentWhere == null) {
        //        return (T)_client.Database.QueryScalar(sql);
        //    }

        //    String sqlWhere = _fluentWhere.GetSql(0);
        //    In[] parametersWhere = _fluentWhere.GetParameters(0);

        //    object x = _client.Database.QueryScalar(String.Format("{0} {1}", sql, sqlWhere), parametersWhere);

        //    return (T) Convert.ChangeType(x, typeof(T));
        //}
    }
}