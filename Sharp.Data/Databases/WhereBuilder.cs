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
			OpenParentesis();
			BuildRecursive((Filter)filter.Left);
			AddLogicOperator(filter);
			BuildRecursive((Filter)filter.Right);
			CloseParentesis();
		}

		private void AddLogicOperator(Filter filter) {
			AddSpace();
            var filterLogic = filter as FilterLogic;
			_builder.Append(LogicOperatorToSymbol.Get(filterLogic.LogicOperator));
			AddSpace();
		}

		private void AddFilterParameter(FilterCondition filter) {
			OpenParentesis();
			AppendParameter(filter.Left);
			AppendCompareOperator(filter);
			AppendParameter(filter.Right);
			CloseParentesis();
		}

		private void AppendParameter(object parameter) {
			var filterParameter = (FilterParameter)parameter;
			if(filterParameter.FilterParameterType == FilterParameterType.Column) {
				_builder.Append(filterParameter.Value);
			}
			else {
			    string value = filterParameter.ValueIsNullOrDBNull ?
			        _dialect.WordNull :
			        _dialect.GetParameterName(_numValues + _parameterStartIndex);
				_builder.Append(value);
				_numValues++;
			}
		}

		private void AppendCompareOperator(Filter filter) {
			var filterCondition = (FilterCondition) filter;
            var compareOperator = ChangeCompareOperatorToIsWhenParameterValueIsNull(filterCondition);
			AddSpace();
            _builder.Append(CompareOperatorToSymbol.Get(compareOperator));
			AddSpace();
		}

        private static CompareOperator ChangeCompareOperatorToIsWhenParameterValueIsNull(FilterCondition filterCondition) {
	        var parameter = filterCondition.Right as FilterParameter;
            if (parameter == null ||
                parameter.FilterParameterType != FilterParameterType.Value ||
                !parameter.ValueIsNullOrDBNull
                ) {
                    return filterCondition.CompareOperator;                
            }
            return CompareOperator.Is;
        }

	    private void OpenParentesis() {
			_builder.Append("(");
		}

		private void CloseParentesis() {
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