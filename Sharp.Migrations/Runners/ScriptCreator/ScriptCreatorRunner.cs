using System;
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
            _versionRepository = new ScriptCreatorVersionRepository(_scriptCreatorDataClient);
            _versionRepository.OnUpdateVersion += UpdateSchemaVersion;

            Runner.Log = new ScriptCreatorLogger(Runner.Log, this);
            _runner = new Runner(_scriptCreatorDataClient, targetAssembly, _versionRepository);
        }

        public void Run(int version, string migrationGroup = null) {
            if (migrationGroup != null) {
                _runner.MigrationGroup = migrationGroup;
            }
            _runner.Run(version);
        }
        
        protected void UpdateSchemaVersion(long version) {
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

        private class ScriptCreatorLogger : ISharpLogger {
            private ISharpLogger _logger;
            private ScriptCreatorRunner _scriptCreatorRunner;

            public bool IsErrorEnabled { get; private set; }
            public bool IsFatalEnabled { get; private set; }
            public bool IsDebugEnabled { get { return true; } }
            public bool IsInfoEnabled { get; private set; }
            public bool IsWarnEnabled { get; private set; }
            
            public ScriptCreatorLogger(ISharpLogger logger, ScriptCreatorRunner scriptCreatorRunner) {
                _logger = logger;
                _scriptCreatorRunner = scriptCreatorRunner;
            }

            public void Info(object message) {
                _logger.Info(message);
                AddComment(message);
            }

            public void Error(object message) {
                _logger.Error(message);
                AddComment(message);                
            }

            public void Warn(object message) {
                _logger.Warn(message);
                AddComment(message);                
            }

            public void Debug(object message) {
                _logger.Debug(message);
                AddComment(message);                
            }

            public void Error(object message, Exception exception) {
                _logger.Error(message, exception);
                AddComment(message); 
            }

            public void ErrorFormat(string format, params object[] args) {
                _logger.ErrorFormat(format, args);
                AddComment(String.Format(format, args));
            }

            public void Fatal(object message) {
                throw new NotImplementedException();
            }

            public void Fatal(object message, Exception exception) {
            }

            public void Debug(object message, Exception exception) {
            }

            public void DebugFormat(string format, params object[] args) {
            }

            public void Info(object message, Exception exception) {
            }

            public void InfoFormat(string format, params object[] args) {
            }

            public void Warn(object message, Exception exception) {
            }

            public void WarnFormat(string format, params object[] args) {
            }

            private void AddComment(object comment) {
                _scriptCreatorRunner.AddScriptComment(comment.ToString());
            }
        }
    }

    
}