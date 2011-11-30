using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Databases.Oracle {
	public class OracleDataClient : DataClient {
		
		public OracleDataClient(IDatabase database) : base(database) {}

		private Dialect _dialect;
		public override Dialect Dialect {
			get {
				if(_dialect == null) {
					_dialect = new OracleDialect();
				}
				return _dialect;
			}
			set { _dialect = value; }
		}
	}
}
