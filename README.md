# embark
Easy to use data storage, for start-ups and personal projects with an agile code-first approach.

Embark is a lightweight document database with only a few commands,
so that you don't have to learn a whole new framework to get going.

Basic:
- Insert(tag, object)
- Update(tag, id, object)
- Get(tag, id)
- Delete(id)

Range:
- UpdateWhere(tag, new object, old object, optional: another old object to form a search range)
- GetWhere(..)
- DeleteWhere(..)

That's it!

The intent of Embark is to stay clean, simple and minimal.
Extra features like database replication, backup, user rights, etc..
will be the core of another project (voyage, coming soon!) so that embark itself will remain crisp and friendly :)

You can add the embark client Nuget package, download and run the server from [here](http://example.todo/), then simply continue developing.
Or if you prefer, download and compile the client library and server - it will only take a minute.

Sample usage:

{
..
}
