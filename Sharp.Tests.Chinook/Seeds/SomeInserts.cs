using System;
using log4net.Config;
using Sharp.Migrations;

namespace Sharp.Tests.Chinook.Seeds {
    public class SomeInserts : SeedMigration {
        public override void Up(string param = null) {
            int num = 30;
            if (!String.IsNullOrEmpty(param)) {
                num = Int32.Parse(param);
            }

            Log("Inserting {0} genres", num);
            var insert = Insert.Into("Genre").Columns("GenreId", "Name");
            for (int i = 11; i <= 11 + num; i++) {
                insert.Values(i, "Genre" + i);
            }
        }
    }
}