namespace SharpMigrations.Tests.Chinnok.Migrations {

    public class _013_Populate_genres : DataMigration {

        public override void Up() {
            Insert
                .Into("Genre")
                .Columns("GenreId", "Name")
                .Values(1, "Rock")
                .Values(2, "Jazz")
                .Values(3, "Metal")
                .Values(4, "Alternative & Punk")
                .Values(5, "Rock And Roll")
                .Values(6, "Blues")
                .Values(7, "Latin")
                .Values(8, "Reggae")
                .Values(9, "Pop")
                .Values(10, "Soundtrack");
        }

        public override void Down() {
            Delete.From("Genre").AllRows();
        }
    }
}