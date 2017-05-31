namespace SharpMigrations.Tests.Chinnok.Migrations {
    public class _012_Create_Foreign_Keys : SchemaMigration {

        public override void Up() {

            Add.ForeignKey("FK_Artist_Album")
               .OnColumn("ArtistId")
               .OfTable("Album")
               .ReferencingColumn("ArtistId")
               .OfTable("Artist")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_Album_Track")
               .OnColumn("AlbumId")
               .OfTable("Track")
               .ReferencingColumn("AlbumId")
               .OfTable("Album")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_MediaType_Track")
               .OnColumn("MediaTypeId")
               .OfTable("Track")
               .ReferencingColumn("MediaTypeId")
               .OfTable("MediaType")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_Genre_Track")
               .OnColumn("GenreId")
               .OfTable("Track")
               .ReferencingColumn("GenreId")
               .OfTable("Genre")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_Employee_ReportsTo")
               .OnColumn("ReportsTo")
               .OfTable("Employee")
               .ReferencingColumn("EmployeeId")
               .OfTable("Employee")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_Employee_Customer")
               .OnColumn("SupportRepId")
               .OfTable("Customer")
               .ReferencingColumn("EmployeeId")
               .OfTable("Employee")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_Customer_Invoice")
               .OnColumn("CustomerId")
               .OfTable("Invoice")
               .ReferencingColumn("CustomerId")
               .OfTable("Customer")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_ProductItem_InvoiceLine")
               .OnColumn("TrackId")
               .OfTable("InvoiceLine")
               .ReferencingColumn("TrackId")
               .OfTable("Track")
               .OnDeleteNoAction();

            Add.ForeignKey("FK_Invoice_InvoiceLine")
              .OnColumn("InvoiceId")
              .OfTable("InvoiceLine")
              .ReferencingColumn("InvoiceId")
              .OfTable("Invoice")
              .OnDeleteNoAction();

            Add.ForeignKey("FK_Track_PlaylistTrack")
              .OnColumn("TrackId")
              .OfTable("PlaylistTrack")
              .ReferencingColumn("TrackId")
              .OfTable("Track")
              .OnDeleteNoAction();

            Add.ForeignKey("FK_Playlist_PlaylistTrack")
              .OnColumn("PlaylistId")
              .OfTable("PlaylistTrack")
              .ReferencingColumn("PlaylistId")
              .OfTable("Playlist")
              .OnDeleteNoAction();
        }

        public override void Down() {

            Remove.ForeignKey("FK_Artist_Album").FromTable("Album");
            Remove.ForeignKey("FK_Album_Track").FromTable("Track");
            Remove.ForeignKey("FK_MediaType_Track").FromTable("Track");
            Remove.ForeignKey("FK_Genre_Track").FromTable("Track");
            Remove.ForeignKey("FK_Employee_ReportsTo").FromTable("Employee");
            Remove.ForeignKey("FK_Employee_Customer").FromTable("Customer");
            Remove.ForeignKey("FK_Customer_Invoice").FromTable("Invoice");
            Remove.ForeignKey("FK_ProductItem_InvoiceLine").FromTable("InvoiceLine");
            Remove.ForeignKey("FK_Invoice_InvoiceLine").FromTable("InvoiceLine");
            Remove.ForeignKey("FK_Track_PlaylistTrack").FromTable("PlaylistTrack");
            Remove.ForeignKey("FK_Playlist_PlaylistTrack").FromTable("PlaylistTrack");
        }
    }
}
