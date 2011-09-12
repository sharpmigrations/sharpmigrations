using System.Collections.Generic;
using Sharp.Data.Query;

namespace Sharp.Data.Filters {
	public class Filter {
		public object Left { get; set; }
		public object Right { get; set; }

		protected Filter() {}

		public object[] GetAllValueParameters() {
			List<object> parameters = new List<object>();
			GetAllValueParametersRecursive(this, parameters);
			return parameters.ToArray();
		}

		private void GetAllValueParametersRecursive(Filter filter, List<object> parameters) {
			GoToSubNode(filter.Left, parameters);
			GoToSubNode(filter.Right, parameters);
		}

		private void GoToSubNode(object node, List<object> parameters) {
			if (node is Filter) {
				GetAllValueParametersRecursive(node as Filter, parameters);
			}
			AddValueParameterToList(node, parameters);
		}

		private void AddValueParameterToList(object filter, List<object> parameters) {
			FilterParameter filterParameter = filter as FilterParameter;
			if (filterParameter == null) {
				return;
			}

			if (filterParameter.FilterParameterType == FilterParameterType.Value) {
				parameters.Add(filterParameter.Value);
				return;
			}
		}

		public static Filter Eq(string columnName, object value) {
			return CreateFilterCondition(CompareOperator.Equals, columnName, value);
		}

		public static Filter Gt(string columnName, object value) {
			return CreateFilterCondition(CompareOperator.GreaterThan, columnName, value);
		}

		public static Filter Lt(string columnName, object value) {
			return CreateFilterCondition(CompareOperator.LessThan, columnName, value);
		}

		public static Filter Ge(string columnName, object value) {
			return CreateFilterCondition(CompareOperator.GreaterOrEqualThan, columnName, value);
		}

		public static Filter Le(string columnName, object value) {
			return CreateFilterCondition(CompareOperator.LessOrEqualThan, columnName, value);
		}

		private static Filter CreateFilterCondition(CompareOperator compareOperator, string columnName, object value) {
			return new FilterCondition {
				CompareOperator = compareOperator,
				Left = new FilterParameter {Value = columnName, FilterParameterType = FilterParameterType.Column},
				Right = new FilterParameter {Value = value, FilterParameterType = FilterParameterType.Value}
			};
		}


		public static Filter And(Filter leftFilter, Filter rightFilter) {
			return CreateFilterLogic(LogicOperator.And, leftFilter, rightFilter);
		}

		public static Filter Or(Filter leftFilter, Filter rightFilter) {
			return CreateFilterLogic(LogicOperator.Or, leftFilter, rightFilter);
		}

		private static Filter CreateFilterLogic(LogicOperator logicOperator, Filter leftFilter, Filter rightFilter) {
			return new FilterLogic {
				Left = leftFilter,
				Right = rightFilter,
				LogicOperator = logicOperator
			};
		}
	}
}