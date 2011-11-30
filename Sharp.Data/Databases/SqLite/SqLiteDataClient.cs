using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sharp.Data.Databases.SqLite {
	public class SqLiteDataClient : DataClient {
		public SqLiteDataClient(IDatabase database) : base(database) {}

		private Dialect _dialect;
		public override Dialect Dialect {
			get {
				if (_dialect == null) {
					_dialect = new SqLiteDialect();
				}
				return _dialect;
			}
			set { _dialect = value; }
		}
	}
}
