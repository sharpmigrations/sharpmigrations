using Sharp.Migrations;

namespace SharpMigrations.Tests.Chinook {
    public class _014_Populate_media_types : DataMigration {

        public override void Up() {
            Insert.Into("MediaType")
                .Columns("MediaTypeId", "Name")
                .Values(1, "MPEG audio file")
                .Values(2, "Protected AAC audio file")
                .Values(3, "Protected MPEG-4 video file")
                .Values(4, "Purchased AAC audio file")
                .Values(5, "AAC audio file"
            );
        }

        public override void Down() {
            Delete.From("MediaType").AllRows();
        }
    }
}
