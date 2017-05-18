using System;
using System.Data.Common;
using System.Reflection;
using SharpData;

namespace SharpMigrations.Runners {
	public class ConsoleRunner {
		
	    public const string SHOW_CURRENT_VERSION = "Your database is at version {0}";
		public const string ASK_FOR_VERSION = "Please, enter a version number or -1 to migrate to the last one ({0})";
		public const string INVALID_NUMBER = "Please, enter a valid number!";
		public const string PRESS_KEY_TO_EXIT = "Press any key to exit";
		public const string DATABASE_ERROR = "Database error:";

		private long _targetVersion;
	    private Runner _runner;

	    public ConsoleRunner(IDataClient dataClient, 
                             Assembly assembly, 
                             string migrationGroup = null) {
            _runner = new Runner(dataClient, assembly, migrationGroup);
	    }

		public void Start() {
			GetInfoFromUser();
			RunMigrations();
			TerminateProgram();
		}

	    protected virtual void GetInfoFromUser() {
	        GetTargetVersion();
	    }

	    protected void GetTargetVersion() {
	        ShowCurrentVersion(_runner.CurrentVersionNumber);
	        _targetVersion = GetLongFromConsole(string.Format(ASK_FOR_VERSION, _runner.LastVersionNumber));
	    }

	    private void RunMigrations() {
			try {
			    _runner.Run(_targetVersion);
            }
			catch (DatabaseException dex) {
				ShowError(dex);
			}
		}
	    
	    private void ShowCurrentVersion(long version) {
            Console.WriteLine(SHOW_CURRENT_VERSION, version);
        }

	    public static int GetIntFromConsole(string message) {
			int version;
			Console.WriteLine(message);
			while (!Int32.TryParse(Console.ReadLine(), out version)) {
				Console.WriteLine(INVALID_NUMBER);
			}
			return version;
		}

        public static long GetLongFromConsole(string message) {
            long version;
            Console.WriteLine(message);
            while (!Int64.TryParse(Console.ReadLine(), out version)) {
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