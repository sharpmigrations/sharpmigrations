namespace Sharp.Data.Fluent {
    public interface IFluentModify {
        ModifyColumn Column(string columnName);
    }

    public class FluentModify : IFluentModify {
        private IDataClient _dataClient;

        public FluentModify(IDataClient dataClient) {
            _dataClient = dataClient;
        }

        public ModifyColumn Column(string columnName) {
            return new ModifyColumn(_dataClient, columnName);
        }
    }
}