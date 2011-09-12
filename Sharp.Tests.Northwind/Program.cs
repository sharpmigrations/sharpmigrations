using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;
using Sharp.Data.Config;

namespace Northwind.Sharp.Migrations {
	class Program {
		static void Main(string[] args) {
			XmlConfigurator.Configure();

			DefaultConfig.IgnoreDialectNotSupportedActions = true;

			NorthwindMigrations migrations = new NorthwindMigrations(DefaultConfig.ConnectionString, DefaultConfig.DatabaseProvider);
			migrations.Start();
		}
	}
}
