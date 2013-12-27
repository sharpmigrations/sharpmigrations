sharpmigrations
===============

Database migrations framework for c#

Sharp Migrations

Sharp migrations is a framework to help you deal with database changes and versioning over time. The idea comes from the excellent Ruby on Rail's migrations and it is the best way to alter your database in a structured and organised manner.

Interested? [Get started!](https://github.com/andrecarlucci/sharpmigrations/wiki/Getting-Started)

**Nuget package: install-package sharpmigrations**

With sharp-migrations you will be writing code like:

```csharp
public class _003_Create_table_Artist : SchemaMigration {
    
    public override void Up() {
        
        Add.Table("Artist").WithColumns(
            Column.AutoIncrement("ArtistId").AsPrimaryKey(),
            Column.String("Name", 120)
        );

        Add.ForeignKey("FK_Artist_Album")
           .OnColumn("ArtistId")
           .OfTable("Album")
           .ReferencingColumn("ArtistId")
           .OfTable("Artist")
           .OnDeleteNoAction();
    }

    public override void Down() {
        Remove.ForeignKey("FK_Artist_Album").FromTable("Album");
        Remove.Table("Artist");
    }
}
```
