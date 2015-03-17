# embark
Easy to use document database with only a few commands,
so that you don't have to learn a whole new framework to get going.. ideal for projects with an agile code-first approach, or if you just want something for now while prototyping.

###basic:
- Insert(object)
- Update(id, object)
- Get(id)
- Delete(id)

###range:
- GetLike(new { Name = "Rocket"})
- UpdateLike(new object, ..) 
- DeleteLike(..)
- GetBetween(new { Score = 15}, new { Score = 39.21})
- UpdateBetween(..)
- DeleteBetween(..)

**That's it!**

You can add the embark client [Nuget package](http://example.todo/), then copy paste the sample code and then simply continue developing right away.

The intent of embark is to stay clean, simple and minimal..
Extra features like database replication, backup, user rights, etc.. will be done in another project ([splash](https://trello.com/splashdb), coming soon!) so that the core embark itself will remain crisp and friendly :)

If you want to save data over a network then download and run the server from [here](http://example.todo/). If you prefer to know the running code, feel free to download and compile - it will only take a minute.

Sample usage:
```csharp
// arrange some guinea pig
var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };

// save data locally
var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\"); /* Client.GetLocalDB() defaults to: Directory.GetCurrentDirectory() */

// or over a network
// var io = Embark.Client.GetNetworkDB("127.0.0.1", 8765);// Not implemented, yet..

// reference collections when used a lot so you don't have to keep typing them out
var io = db["sheep"];

// insert
long id = io.Insert(pet);

// get
Sheep fluffy = io.Select<Sheep>(id);

// update
fluffy.FavouriteIceCream = IceCream.Strawberry;
bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

// delete
bool hasSheepVanished = io.Delete(id);
```

To check out current developments go to [embarkdb on trello](https://trello.com/embarkdb)
