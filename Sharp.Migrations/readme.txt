PLEASE, READ THIS BEFORE RUNNING YOUR MIGRATIONS!!!
---------------------------------------------------
From version 1.3 to 1.4, we've made many changes on the schema_version table.

- The version table now is called sharpmigrations
- Each migration is stored in a separated line in this table among with some info
- If your version is 10, and for some reason the migration version 8 wasn't run, it will be run next time you migrate your database

Updating from schema_version to sharpmigration table
- This will be done automatically next time you migrate your database.
- The table schema_version will be deleted
- The table sharpmigration will be created and populated with the data from your migrations

It's a safe procedure, but as always, take care and backup your database before doing this.