using Sharp.Data.Query;

namespace Sharp.Data.Filters {

	public class CompareOperatorToSymbol {

		public static string Get(CompareOperator compareOperator) {
			switch (compareOperator) {
				case CompareOperator.Equals:
					return "=";
				case CompareOperator.GreaterOrEqualThan:
					return ">=";
				case CompareOperator.GreaterThan:
					return ">";
				case CompareOperator.LessOrEqualThan:
					return "<=";
				case CompareOperator.LessThan:
					return "<";
                case CompareOperator.Is:
                    return "is";
				default:
					return "";
			}
		}
	}
}