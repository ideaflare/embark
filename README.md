# embark
Easy to use document database with only a few commands,
so that you don't have to learn a whole new framework to get going.. ideal for projects with an agile code-first approach, or if you just want something for now while prototyping.

###basic:
- Insert(tag, object)
- Update(tag, id, object)
- Get(tag, id)
- Delete(tag, id)

###range:
- UpdateWhere(tag, new object, old object, optional: another old object to form a search range)
- GetWhere(..)
- DeleteWhere(..)

**That's it!**

You can add the embark client [Nuget package](http://example.todo/), then copy paste the sample code and then simply continue developing right away.

The intent of embark is to stay clean, simple and minimal, without compromising speed, consistency and concurrency. Extra features like database replication, backup, user rights, etc.. will be done in another project ([voyage](http://example.todo/), coming soon!) so that the core embark itself will remain crisp and friendly :)

If you want to save data over a network then download and run the server from [here](http://example.todo/). If you prefer to know the running code, feel free to download and compile - it will only take a minute.

Sample usage:
```csharp
// arrange some guinea pig
var tag = "sheep";
var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };
 
// save data locally
var io = new Embark.Client(@"C:\MyTemp\Embark");

// or over a network
var io = new Embark.Client("127.0.0.1", 80);// Not implemented, yet..

// insert
long id = io.Insert(tag, pet);

// get
Sheep fluffy = io.Get<Sheep>(tag, id);

// update
fluffy.FavouriteIceCream = IceCream.Strawberry;

bool hasSheepUpdated = io.Update(tag, id, fluffy);

// delete
bool hasSheepVanished = io.Delete(tag, id);
```

To check out current developments go to [embarkdb on trello](https://trello.com/embarkdb)
