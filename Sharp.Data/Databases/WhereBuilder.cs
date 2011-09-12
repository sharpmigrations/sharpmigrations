using System.Text;
using Sharp.Data.Filters;
using Sharp.Data.Query;

namespace Sharp.Data.Databases {
	public class WhereBuilder {

		private StringBuilder _builder = new StringBuilder();
		private Dialect _dialect;
		private int _numValues;
		private int _parameterStartIndex;

		public WhereBuilder(Dialect dialect, int parameterStartIndex) {
			_dialect = dialect;
			_parameterStartIndex = parameterStartIndex;
		}

		public virtual string Build(Filter filter) {

			AppendWordWhere();

			BuildRecursive(filter);

			return _builder.ToString();
		}

		private void BuildRecursive(Filter filter) {
			if (filter is FilterCondition) {
				AddFilterParameter((FilterCondition)filter);
				return;
			}

			OpenBrackets();
			
			BuildRecursive((Filter)filter.Left);

			AddLogicOperator(filter);

			BuildRecursive((Filter)filter.Right);
			
			CloseBrackets();
		}

		private void AddLogicOperator(Filter filter) {
			AddSpace();
			FilterLogic filterLogic = filter as FilterLogic;
			_builder.Append(LogicOperatorToSymbol.Get(filterLogic.LogicOperator));
			AddSpace();
		}

		private void AddFilterParameter(FilterCondition filter) {
			OpenBrackets();

			AppendParameter(filter.Left);

			AppendCompareOperator(filter);

			AppendParameter(filter.Right);

			CloseBrackets();
		}

		private void AppendParameter(object parameter) {
			FilterParameter filterParameter = (FilterParameter)parameter;
			
			if(filterParameter.FilterParameterType == FilterParameterType.Column) {
				_builder.Append(filterParameter.Value);
			}
			else {
				_builder.Append(_dialect.GetParameterName(_numValues + _parameterStartIndex));
				_numValues++;
			}
		}

		private void AppendCompareOperator(Filter filter) {
			FilterCondition filterCondition = (FilterCondition) filter;
			AddSpace();
			_builder.Append(CompareOperatorToSymbol.Get(filterCondition.CompareOperator));
			AddSpace();
		}

		private void OpenBrackets() {
			_builder.Append("(");
		}

		private void CloseBrackets() {
			_builder.Append(")");
		}

		private void AddSpace() {
			_builder.Append(" ");
		}

		private void AppendWordWhere() {
			_builder.Append(_dialect.WordWhere).Append(" ");
		}
	}
}