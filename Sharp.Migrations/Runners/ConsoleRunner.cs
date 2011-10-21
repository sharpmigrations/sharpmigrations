using System;
using System.Reflection;
using Sharp.Data;

namespace Sharp.Migrations.Runners {
	public class ConsoleRunner {
		protected string _connectionString;
		protected string DatabaseProvider;

		public const string ASK_FOR_VERSION = "Please, enter a version number or -1 to migrate to the last one";
		public const string INVALID_NUMBER = "Please, enter a valid number!";
		public const string PRESS_KEY_TO_EXIT = "Press any key to exit";
		public const string DATABASE_ERROR = "Database error:";

		protected int _targetVersion;
		protected Assembly _assemblyWithMigrations;

		public Assembly AssemblyWithMigrations {
			get { return _assemblyWithMigrations ?? Assembly.GetEntryAssembly(); }
			set { _assemblyWithMigrations = value; }
		}

        public string MigrationGroup { get; set; }

		public ConsoleRunner(string connectionString, string databaseProvider) : this(connectionString, databaseProvider, null) {}

        public ConsoleRunner(string connectionString, string databaseProvider, string migrationGroup) {
            _connectionString = connectionString;
            DatabaseProvider = databaseProvider;
            if(!String.IsNullOrEmpty(migrationGroup)) {
                MigrationGroup = migrationGroup;
            }
        }

		public void Start() {
			GetInfoFromUser();
			RunMigrations();
			TerminateProgram();
		}

	    protected virtual void GetInfoFromUser() {
	        GetTargetVersion();
	    }

	    private void RunMigrations() {
			try {
				TryRunMigrations();
			}
			catch (DatabaseException dex) {
				ShowError(dex);
			}
		}

	    protected virtual void TryRunMigrations() {
	        IDataClient dataClient = SharpFactory.Default.CreateDataClient(_connectionString, DatabaseProvider);
            Runner runner = new Runner(dataClient, AssemblyWithMigrations);
            if(MigrationGroup != null) {
                runner.MigrationGroup = MigrationGroup;                
            }
	        runner.Run(_targetVersion);
	    }

	    protected void GetTargetVersion() {
			_targetVersion = GetIntFromConsole(ASK_FOR_VERSION);
		}

	    protected int GetIntFromConsole(string message) {
			int version;
			Console.WriteLine(message);
			while (!Int32.TryParse(Console.ReadLine(), out version)) {
				Console.WriteLine(INVALID_NUMBER);
			}
			return version;
		}

	    protected virtual void ShowError(DatabaseException dex) {
			Console.WriteLine(DATABASE_ERROR + dex.Message);
			if (dex.SQL.Length > 0) {
				Console.WriteLine("------------------------------");
				Console.WriteLine("Sql:");
				Console.WriteLine(dex.SQL);
				Console.WriteLine("------------------------------");
			}
		}

		private void TerminateProgram() {
			Console.WriteLine(PRESS_KEY_TO_EXIT);
			Console.ReadLine();
		}
	}
}