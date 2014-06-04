Version 1.4.7

New features:
---------------------------------------------------------------------------------------------
PowerShell command: Update-Database
---------------------------------------------------------------------------------------------
Just type Update-Database in your Package Manager Console and your database will be migrated.
The params are:

-version          : the version to migrate (default latest)
-assemblyPath     : assembly with migrations. (default current project)
-connectionString : database connectionstring. If not set, a menu with all connection strings 
					from app.config will be shown
-provider		  : database provider (see Sharp.Data.Databases.DataProviderNames)
-group			  : migration group (default if not set)

---------------------------------------------------------------------------------------------
SharpMigrator
---------------------------------------------------------------------------------------------
SharpMigrator.exe is available in tools directory to run your migrations in your CI server.

---------------------------------------------------------------------------------------------
No migration left behind
---------------------------------------------------------------------------------------------
From this version on, all migrations have their own register in the database. This way, even
if you get a migration not applied in your sandbox (a merge for instance), it will be applied
next time you migrate the database

---------------------------------------------------------------------------------------------
PLEASE, READ THIS BEFORE RUNNING YOUR MIGRATIONS IF YOU'RE UPGRADING FROM 1.3
---------------------------------------------------------------------------------------------
From version 1.3 to 1.4, we've made many changes on the schema_version table.

- The version table now is called sharpmigrations
- Each migration is stored in a separated line in this table among with some info
- If your version is 10, and for some reason the migration version 8 wasn't run, it will be 
  run next time you migrate your database

Updating from schema_version to sharpmigration table
- This will be done automatically next time you migrate your database.
- The table schema_version will be deleted
- The table sharpmigration will be created and populated with the data from your migrations

It's a safe procedure, but as always, take care and backup your database before doing this.