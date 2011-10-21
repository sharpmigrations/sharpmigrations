using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;

namespace Northwind.Sharp.Migrations {
	class Program {
		static void Main(string[] args) {
			XmlConfigurator.Configure();

			NorthwindMigrations migrations = new NorthwindMigrations();
			migrations.Start();
		}
	}
}
