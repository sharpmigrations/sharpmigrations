using System;
using Sharp.Data.Query;

namespace Sharp.Data.Filters {
	public static class LogicOperatorToSymbol {

		public static string Get(LogicOperator logicOperator) {
		    return logicOperator.ToString();
		}
	}
}
