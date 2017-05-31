using System;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using SharpData;

namespace SharpMigrations.Runners.ScriptCreator {
    public class ScriptCreatorRunner {

        private Dialect _dialect;
        private ScriptCreatorDatabase _scriptCreatorDatabase;
        private StringBuilder _script = new StringBuilder();
        private Runner _runner;

        public ScriptCreatorRunner(IDataClient dataClient, 
                                   Assembly targetAssembly,
                                   string migrationGroup) {
            _dialect = dataClient.Dialect;
            _scriptCreatorDatabase = new ScriptCreatorDatabase(_dialect, dataClient.Database);
            var scriptCreatorDataClient = new DataClient(_scriptCreatorDatabase, _dialect);
            var versionRepository = new ScriptCreatorVersionRepository(scriptCreatorDataClient, migrationGroup);
            versionRepository.OnUpdateVersion += UpdateSchemaVersion;

            Runner.Logger = new ScriptCreatorLogger(Runner.Logger, this);
            _runner = new Runner(scriptCreatorDataClient, targetAssembly, versionRepository);
        }

        public void Run(long version) {
            _runner.OnMigrationError += (s, a) => {
                a.Handled = true;
                _script.AppendLine("------------------------------------------------------");
                _script.AppendLine("--Error running Migration " + a.MigrationName);
                _script.AppendLine("--Error: " + a.Exception.Message);
                _script.AppendLine("------------------------------------------------------");
            };
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

        private class ScriptCreatorLogger : ILogger {
            private ILogger _logger;
            private ScriptCreatorRunner _scriptCreatorRunner;

            public ScriptCreatorLogger(ILogger logger, ScriptCreatorRunner scriptCreatorRunner) {
                _logger = logger;
                _scriptCreatorRunner = scriptCreatorRunner;
            }

            public IDisposable BeginScope<TState>(TState state) {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel) {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
                _logger.Log(logLevel, eventId, state, exception, formatter);
                AddComment(formatter.Invoke(state, exception));
            }
            private void AddComment(object comment) {
                _scriptCreatorRunner.AddScriptComment(comment.ToString());
            }
        }
    }
}