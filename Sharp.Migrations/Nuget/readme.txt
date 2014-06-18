Version 1.5

New features:
---------------------------------------------------------------------------------------------
Seeds
---------------------------------------------------------------------------------------------
Seeds are data migrations completely independent that are not versioned. They are very 
usefull to create specific scenarios for your database.
For instance, you can create a DevSeed that when called adds default users and data specific
for testing.

Create any class that extends SeedMigrations. You can put it anywhere. No need for version 
numbers. To run this seed, use the Package Manager Console (Invoke-Seed command, see below) 
or the SharpMigrator.exe.

Seeds have no "Down()" method. If you want to revert changes of a Seed, create another Seed
that undo the changes.

You can pass an argument to your seed too. The argument is always a string and it's up to you
to parse and use it the way you want. Ex: you could pass the number of dummy users you want
to create in some Seed.

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
PowerShell command: Invoke-Seed
---------------------------------------------------------------------------------------------
Invoke-Seed applies some seed to the database.
The params are:

-seedName         : name of the seed to apply (mandatory)
-seedArgs		  : some argument to pass to the seed (default nothing)
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