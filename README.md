# Sharp Migrations

Sharp migrations is a framework to help you deal with database changes and versioning over time. The idea comes from the excellent Ruby on Rail's migrations and it is the best way to alter your database in a structured and organised manner.

**Nuget package: install-package sharpmigrations**

## Introduction

Sharp migrations is a framework to help you deal with database changes and versioning over time. The idea comes from the excellent Ruby on Rail's migrations and it is the best way to alter your database in a structured and organised manner.

## How it works?

The idea is simple: create a class for every change you want to make on your database. That class will handle which actions to take for applying the new modification and for removing it if necessary in the future. Every class represents a version for our database versioning system.

## Schema Migration

Cool, can you show me some code? 
Sure! Let's suppose you want to create a new table called Artist. This is a *schema migration* and as I said, you have to write the code for *adding the table* and for *removing it*. So take a look at this code:

```csharp
public class _001_CreateTableArtist : SchemaMigration { 
  
  public override void Up() {        
     Add.Table("Artist")
        .WithColumns( 
            Column.AutoIncrement("ArtistId").AsPrimaryKey(), 
            Column.String("Name", 120).NotNull()
        ); 
  } 

  public override void Down() { 
     Remove.Table("Artist"); 
  } 
}
```

Since this is a schema migration, we created a class that extends SchemaMigration. You have two methods to override: Up() and Down().

The method Up() is called every time you want to change your database one version up. The method Down() is called, well, you can figure out, right?

## Reversible Schema Migration

There are cases where you don't need to write the "down" method of a migration because SharpMigrations can figure it out based on the actions you perform on the "up" method.

Example:

```csharp
public class _001_Create_table_revfoo_with_comments : ReversibleSchemaMigration {
	public override void Up() {
		Add.Table("revfoo")
		   .WithColumns(
			   Column.AutoIncrement("id").Comment("This is the id"),
			   Column.String("name").NotNull().Comment("This is the name")
			);
		Add.Comment("This table is cool!").ToTable("revfoo");
	}
}
```

SharpMigrations will create the "drop table" command for you. The only thing you need to do is to extend the ReversibleSchemaMigration class instead of the SchemaMigration class.

The commands you can in this class are:

* Add Column
* Add Table
* Add PrimaryKey
* Add ForeignKey
* Add UniqueKey
* Add IndexKey
* Add Comment
* Rename Table
* Rename Column

## Data Migrations

Can I make changes on data as well?

Yes you can! Suppose you want to add initial values to our Artist table. Since it's a data migration, you will extend the DataMigration class.

```csharp
public class _002_PopulateTableArtist : DataMigration {

    public override void Up() {
        Insert.Into("Artist")
              .Columns("ArtistId", "Name")
              .Values(1, "Zero 7");
    }

    public override void Down() {
        Delete.From("Artist")
              .Where(
                  Filter.Eq("ArtistId", 1)
              );
    }
}
```


## Seeds

Seeds are data migrations completely independent that are not versioned. They are very 
usefull to create specific scenarios for your database.
For instance, you can create a DevSeed that when called adds default users and data specific
for testing.

Create any class that extends SeedMigrations. You can put it anywhere. No need for version 
numbers. To run this seed, use the Package Manager Console (Invoke-Seed command) 
or the SharpMigrator.exe.

Seeds have no "Down()" method. If you want to revert changes of a Seed, create another Seed
that undo the changes.

You can pass an argument to your seed too. The argument is always a string and it's up to you
to parse and use it the way you want. Ex: you could pass the number of dummy users you want
to create in some Seed.

Ex:

```csharp
    public class DummyGenresForTesting : SeedMigration {
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
```

In Package Manager Console, type:

    Invoke-Seed DummyGenresForTesting -seedArgs 20

And it will insert 20 new records.


## Versioning

I noticed that you put the version number as prefix of the class name, is it a rule?
Well, the rule is simpler than that: you can put the version number anywhere in the class name.

The framework will scan the class name until it gets the first number on it. That's it. No ugly attributes or fixed places.

Valid names are:

*   __001Foobar
*   Foobar001
*   Foo001bar
*   Foo_001_bar

*Remember*: it's the first number! So Migration001_foo_002 will be the version 1.

Some people like to use incremental numbers like 1, 2, 3, etc, but if you're working in a team, I suggest you use dates to version your migrations. This will avoid two developers commiting two migrations with the same version number.

Ex: public class Migration_201401011058 meaning:
Year 2014
Month 01
Day 01
Hour 10
Minute 58

It doesn't matter for the framework which way you choose as long as the numbers are ordered (for sharp migrations, it's just a Int64).

## Running the migrations

### Use the SharpMigrator.exe
Check the tools folder of your nuget package, the SharpMigrator.exe is there. You can use it to run the migrations in your CI server.

#### Migrating to specific version:

    SharpMigrations -a MyAssembly.dll -c "connectionString" -p "Oracle.ManagedDataAccess.Client" -m auto -v 10 

*Migrates to version 10*

#### Migrating to the latest version using Oracle

    SharpMigrations -a MyAssembly.dll -c "connectionString" -p "Oracle.ManagedDataAccess.Client" -m auto 

#### Generating scripts for running them later by some DBA

    SharpMigrations -a MyAssembly.dll -c "connectionString" -p "Oracle.ManagedDataAccess.Client" -m script -f myScript.txt 

### Inside Visual Studio
You can use nuget powershell commands to run the migrations. Just open the Package Manager Console, point (dropdown) to the project with the migrations and type:

    Update-Database

Migrates to the latest version

    Update-Database -v 2

Migrates to version 2

### Create a console app
You can make your own customized runner creating a console application and extending some Runner. There are many examples in the source code.


##And don't forget that SharpMigrations is powered by:
<a href="http://www.jetbrains.com/resharper" rel="Resharper">![Foo](http://blog.jetbrains.com/wp-content/uploads/2014/04/logo_resharper.gif)</a>

