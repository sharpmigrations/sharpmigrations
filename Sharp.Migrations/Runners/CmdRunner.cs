using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sharp.Data;
using Sharp.Data.Log;

namespace Sharp.Migrations.Runners {
    public class CmdRunner : Runner {

        private IDataClient _dataClientForMigration;
        private SqlToFileDatabase _sqlToFileDatabase;
        private StringBuilder _script = new StringBuilder();

        public CmdRunner(IDataClient dataClient, Assembly targetAssembly) : base(dataClient, targetAssembly) {
            Dialect dialect = dataClient.Dialect;
            _sqlToFileDatabase = new SqlToFileDatabase(dialect);
            _dataClientForMigration = new DataClient(_sqlToFileDatabase, dialect);
            Log = new CmdRunnerLogger(this);
        }

        protected override IDataClient GetDataClientForMigration() {
            return _dataClientForMigration;
        }

        protected override void UpdateVersion(int i) {
            foreach (var sql in _sqlToFileDatabase.Sqls) {
                _script.AppendLine(sql);
            }
            _sqlToFileDatabase.Sqls.Clear();
        }

        public void AddScriptComment(string line) {
            _script.AppendLine(line);
        }

        public List<string> GetAllExecutedSqls() {
            return _sqlToFileDatabase.Sqls;
        }

        private class CmdRunnerLogger : ILogger {
            private CmdRunner _cmdRunner;

            public CmdRunnerLogger(CmdRunner cmdRunner) {
                _cmdRunner = cmdRunner;
            }

            public void Info(string message) {
                throw new NotImplementedException();
            }

            public void Error(string message) {
                throw new NotImplementedException();
            }

            public void Warn(string message) {
                throw new NotImplementedException();
            }

            public void Debug(string message) {
                throw new NotImplementedException();
            }

            public bool IsDebugEnabled { get; private set; }

            private void AddComment(string comment) {
                _cmdRunner.AddScriptComment(comment);
            }
        }
    }

    
}