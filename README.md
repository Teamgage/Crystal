# Crystal - Shard Management for .NET Core web apps
**Author:** The Teamgage Dev Team (https://www.teamgage.com/)

# About
This library provides the capability to easily switch databases at the Data Access Layer based on an element of your choosing (e.g. query/route parameter, part of a token, etc.) present in incoming web requests. It also provides the ability to migrate the individual databases one at a time.

# Getting Started
Crystal requires very minimal configuration and provides a simple integration with the .NET ServiceCollection

## Adding Sharded DBs and Data Dependent Routing
**1. Register the Dependencies:**

When building your IoC service collection, simply call the `.UseCrystal<TKey, TDbProvider>(optionsBuilder)` extension, which will register the required dependencies.

This function accepts two type arguments and one parameter:
* `TKey` is the type of your shard _key_. That is, the unique identifier for each database shard. Currenlty `int`, `long` and `string` types are supported.
* `TDbProvider` is effectively the flavour of your database. It can be any class which implements the `IDbProvider` interface. Crystal includes SQL Server and SQLite in-memory implementations for you to use, but feel free to create your own!
* `optionsBuilder` is an action which allows you to customise your set up of Crystal, e.g. to use different underlying storage mechanisms for keeping track of your shards, or to exclude particular routes from being sharded.

**2. Register the Middleware:**

In your Configure() method in Startup.cs, simply call:

```app.UseMiddleware<CrystalMiddleware<TKey>>();```

Where TKey is the type of key you are using.

**3. Inherit from ShardedDbContext:**

Finally, simply make your EFCore Context class inherit from the `ShardedDbContext` class (again providing the type of the key being used). You will need to implement a constructor in your App Context which will call the base constructor. Luckily if you've got your DI set up correctly, this should all be taken care of by the registrations made in step 1.

## Using the ShardMigrator

The `ShardMigrator` class allows you to easily migrate individual shards by providing the desired key of the database to update. Again, registration of this is relatively simple.
At the end of your `.UseCrystal()` call in step 1 above, simply add the inbuilt `.WithShardMigrator<TKey, TContext>(contextFactory)` call afterwards.

This function also accepts two type arguments and one parameter:
* `TKey`: The type of your shard key (see step 1)
* `TContext`: The type of your context class
* `contextFactory`: A function which will receive an `IShardManager<TKey>` and an `IDbProvider` and return an instance of your `TContext`. This is done in a factory-format in case your context relies on any other dependencies.

## Using the EntityFrameworkCore CLI/Package Manager Commands
To use the nice EF Core utilities such as the CLI or Package Manager commands, we need to have a context class with a parameterless constructor. Because our `ShardedDbContext` relies on two dependencies, you will need to provide an implementation of Microsoft's `IDesignTimeDbContextFactory` in order to use these commands.
Luckily, we've built a wrapper around this interface so that you can simply write a class (somewhere in the same project as your Context class) which inherits from `ShardedDbContextFactory`. If you implement a constructor which calls the base constructor with the following parameters:
* `connectionString`: The connection string of the DB you want to connect to
* `dbProvider`: An instance of your database provider (remember the built-in ones in Crystal, unless you're using your own)
* `contextFactory`: A function to return an instance of your Context class.

the EFCore commands will work as normal :)

## Sample Application
Checkout the Crystal.TestWebApp for a basic example on how to set everything up!
