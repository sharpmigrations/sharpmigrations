using Sharp.Data.Filters;

namespace Sharp.Data.Fluent {
	public class Count : DataClientAction {

		public Filter Filter { get; set; }

		public int CountedRows { get; set; }

		public Count(IDataClient dataClient) : base(dataClient) { }

		protected override void ExecuteInternal() {
            CountedRows = DataClient.CountSql(_tableNames[0], Filter);
		}
	}
}