using System.Reflection;
using System.Text;
using Sharp.Data;
using Sharp.Data.Log;

namespace Sharp.Migrations.Runners.ScriptCreator {
    public class ScriptCreatorRunner {

        private Dialect _dialect;
        private IDataClient _scriptCreatorDataClient;
        private ScriptCreatorDatabase _scriptCreatorDatabase;
        private StringBuilder _script = new StringBuilder();
        private ScriptCreatorVersionRepository _versionRepository;
        private Runner _runner;

        public ScriptCreatorRunner(IDataClient dataClient, Assembly targetAssembly) {
            _dialect = dataClient.Dialect;
            _scriptCreatorDatabase = new ScriptCreatorDatabase(_dialect, dataClient.Database);
            _scriptCreatorDataClient = new DataClient(_scriptCreatorDatabase, _dialect);
            _versionRepository = new ScriptCreatorVersionRepository(_scriptCreatorDataClient, false);
            _versionRepository.OnUpdateVersion += UpdateSchemaVersion;
            
            Runner.Log = new ScriptCreatorLogger(this);
            _runner = new Runner(_scriptCreatorDataClient, targetAssembly, _versionRepository);
        }

        public void Run(int version) {
            _runner.Run(version);
        }
        
        protected void UpdateSchemaVersion(int version) {
            foreach (var sql in _scriptCreatorDatabase.Sqls) {
                AddScript(sql);
            }
            _script.AppendLine();
            _scriptCreatorDatabase.Sqls.Clear();
        }

        public void AddScript(string sql) {
            _script.AppendLine(sql);
            _script.AppendLine(_dialect.ScriptSeparator);
        }

        public void AddScriptComment(string line) {
            _script.Append(_dialect.ScriptCommentsPrefix);
            _script.AppendLine(line);
        }

        public string GetCreatedScript() {
            return _script.ToString();
        }

        private class ScriptCreatorLogger : ILogger {
            private ScriptCreatorRunner _scriptCreatorRunner;

            public ScriptCreatorLogger(ScriptCreatorRunner scriptCreatorRunner) {
                _scriptCreatorRunner = scriptCreatorRunner;
            }

            public void Info(string message) {
                AddComment(message);
            }

            public void Error(string message) {
                AddComment(message);                
            }

            public void Warn(string message) {
                AddComment(message);                
            }

            public void Debug(string message) {
                AddComment(message);                
            }

            public bool IsDebugEnabled { get { return true; } }

            private void AddComment(string comment) {
                _scriptCreatorRunner.AddScriptComment(comment);
            }
        }
    }

    
}