#Idea
Easy to use document database with a focus on visibility and simplicity. Made for prototyping or projects with an agile code-first approach. Embark allows you to [defer the plumbing or commitment of choosing a specific database technology](https://www.youtube.com/watch?v=asLUTiJJqdE).

##Visibility

Documents are saved as [JSON](http://en.wikipedia.org/wiki/JSON#Data_types.2C_syntax_and_example) text files in folders for each collection, so you can easily view/edit data while developing.

![Documents saved as text files](https://trello-attachments.s3.amazonaws.com/54f89e2e643f7e862d27cf6b/551x192/386d7e8b7a80eb55380f4b733d0e8ca6/explore.png "Documents saved as text files")

##Simplicity

```csharp
// arrange some guinea pig
var pet = new Sheep { Name = "Fluffy", FavouriteIceCream = IceCream.Vanilla };

// save data locally
var db = Embark.Client.GetLocalDB(@"C:\AnimalsDB\");

// or over a network via REST API to WCF server *see usage section below*
var db = Embark.Client.GetNetworkDB("192.168.1.24", 8080);

// collections created on-the-fly if needed
var io = db.GetCollection<Sheep>("sheep");

// create
long id = io.Insert(pet);

// read
Sheep fluffy = io.Get(id);

// update
fluffy.FavouriteIceCream = IceCream.Strawberry;
bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

// delete
bool hasSheepVanished = io.Delete(id);

// non-type specific if you want to mix Apples & Oranges objects in the same collection
var io = db["fruit"];
```
###Some other commands are

- GetWhere(new { Name = "Rocket"})
- GetBetween(new { Score = 15}, new { Score = 39.21})
- GetAll() returns Document Wrapper with ID & timestamp
- byte[] GetByteArray(object blob) to help with blob deserialization
- See the [wiki for details](https://github.com/ubrgw/embark/wiki/SQL-Equivalents)

##That's it!

The intent of Embark is to stay simple to use and minimal.
A single .dll under 50 kilobytes and no external dependencies other than the .NET framework. 
By avoiding increasing complexity from scope-creep, Embark will remain crisp and friendly :)

The aim of the project is to provide a DB [while you are building new ideas](https://github.com/ubrgw/embark/wiki) and not to weigh features that do not contribute to prototyping like database replication, query optimizations, optimal disk usage, etc..

![alt text](https://trello-attachments.s3.amazonaws.com/54f89e2e643f7e862d27cf6b/675x592/ed7a589d1d7470a4eaaa8ded3dfa699c/iterativeB.png "Stepping stone development")

##Usage

You can add the [Embark NuGet package](https://www.nuget.org/packages/Embark/), copy paste the sample code, and just continue developing.

If you want to save data over a network:
```csharp
// start a new server
var server = new Embark.Server();
server.Start();
```
or you can download a simple server from [here](https://trello-attachments.s3.amazonaws.com/54f89f538ec1e186a911c534/5527fc8a8a55d94cbed0ab17/a329633e001aae8b8b8deac9fcc2f98b/EmbarkDemo.zip). 
> *NOTE: For the WCF server to work, either run it in admin mode or give access rights to the [your-machine:port/embark/](http://stackoverflow.com/a/17242260/4650900) namespace.*

##[In development:](https://trello.com/b/rtqlPmrM/development)
- Custom text-to-object mapping, like [YAML](http://www.yaml.org/start.html) or even [DSON](http://dogeon.org/)
- Optional convention based collection
- Optional caching & async data persistency
- Review & Simplify code

Check out [Embark kanban on trello](https://trello.com/b/rtqlPmrM/development).

Feel free to contact EmbarkDB@gmail.com with any feedback, suggestions or to get involved!
