using System;
using System.Collections.Generic;
using Sharp.Migrations;
using Sharp.Data;

namespace Sharp.Tests.Migrations {
	public static class MigrationTestHelper {

		public static int VersionForException { get; set; }
		public static int VersionForNotSupportedByDialectException { get; set; }

		static MigrationTestHelper() {
			ExecutedMigrationsUp = new List<Migration>();
			ExecutedMigrationsDown = new List<Migration>();
		}
		
		public static List<Type> GetMigrations() {
			return new List<Type> {
				typeof (Migration1),
				typeof (Migration2),
				typeof (Migration3),
				typeof (Migration5),
				typeof (Migration6)
			};
		}

		public static List<Migration> ExecutedMigrationsUp { get; private set; }
		public static List<Migration> ExecutedMigrationsDown { get; private set; }

		public static void RegisterUp(MigrationBase migrationBase) {
			ExecutedMigrationsUp.Add(migrationBase);
			CheckForExceptions(migrationBase);
		}

		public static void RegisterDown(MigrationBase migrationBase) {
			ExecutedMigrationsDown.Add(migrationBase);
			CheckForExceptions(migrationBase);
		}

		private static void CheckForExceptions(MigrationBase migrationBase) {
			if (VersionForException == migrationBase.Version) {
				throw new Exception();
			}
			if (VersionForNotSupportedByDialectException == migrationBase.Version) {
				throw new NotSupportedByDialect("m","f","d");
			}
		}

		public static void Clear() {
			ExecutedMigrationsUp.Clear();
			ExecutedMigrationsDown.Clear();
			VersionForException = -1;
			VersionForNotSupportedByDialectException = -1;
		}
	}

	public abstract class MigrationBase : Migration {

		public IDataClient DataClient {
			get { return base.DataClient; }
		}

		public override void Up() {
			MigrationTestHelper.RegisterUp(this);
		}

		public override void Down() {
			MigrationTestHelper.RegisterDown(this);			
		}
	}

	public class Migration5 : MigrationBase { }
	public class Migration4 : MigrationBase { }
	public class Migration1 : MigrationBase { }
	public class Migration2 : MigrationBase { }
	public class Migration3 : MigrationBase { }
	public class Migration6 : MigrationBase { }
}