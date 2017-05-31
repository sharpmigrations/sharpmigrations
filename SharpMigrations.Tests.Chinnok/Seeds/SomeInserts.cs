using System;

namespace SharpMigrations.Tests.Chinnok.Seeds {
    public class SomeInserts : SeedMigration {
        public override void Up(string param = null) {
            var num = 30;
            if (!String.IsNullOrEmpty(param)) {
                num = Int32.Parse(param);
            }
            Log("Inserting {0} genres", num);
            var insert = Insert.Into("Genre").Columns("GenreId", "Name");
            for (var i = 11; i <= 11 + num; i++) {
                insert.Values(i, "Genre" + i);
            }
        }
    }
}