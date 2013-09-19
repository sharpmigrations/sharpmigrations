using Sharp.Data.Query;

namespace Sharp.Data.Filters {
	public class FilterParameter {
		public object Value { get; set; }
		public FilterParameterType FilterParameterType { get; set; }
	    public bool ValueIsNullOrDBNull {
	        get {
	            return Value == null || Value == System.DBNull.Value;
	        }
	    }
	}
}