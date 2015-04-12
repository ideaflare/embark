# embark
Easy to use document database with a focus on visibility and simplicity.

Made for prototyping or projects with an agile code-first approach.
Embark allows you to [defer the plumbing or commitment of choosing a specific database technology](https://www.youtube.com/watch?v=asLUTiJJqdE).

##Visibility

Documents are saved in .txt files in folders for each collection so you can easily view/edit data while developing. The default serialization is [JSON](http://en.wikipedia.org/wiki/JSON#Data_types.2C_syntax_and_example), and you can also plug in some other format like [YAML](http://www.yaml.org/start.html) or even [DSON](http://dogeon.org/).

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

// insert
long id = io.Insert(pet);

// get
Sheep fluffy = io.Select(id);

// update
fluffy.FavouriteIceCream = IceCream.Strawberry;
bool fluffyNowLikesStrawberry = io.Update(id, fluffy);

// delete
bool hasSheepVanished = io.Delete(id);

// non-type specific if you want to mix Apples & Oranges objects in the same collection
var io = db["fruit"];
```
###All the commands are

####basic:
- Insert(object) returns Int64 ID
- Select(id) returns document
- Update(id, object) returns bool successful
- Delete(id) returns bool successful

####range:
- SelectLike(new { Name = "Rocket"})
- SelectBetween(new { Score = 15}, new { Score = 39.21})
- SelectAll() returns Document Wrapper with ID & timestamp

####other:
- byte[] GetByteArray(object blob) to help with blob deserialization

####[in development](https://trello.com/b/rtqlPmrM/development):
- Aggregate functions (Count/Average/Min/Max..)
- More/better feedback from server
- Review & Simplify code

###That's it!###

The intent of embark is to stay simple to use and minimal.
Just one .dll under 50 kilobytes and no external dependencies other than the .NET framework. 

The aim of the project is to provide a DB while you are building new ideas and not to weigh features that do not contribute to prototyping like database replication, query optimizations, optimal disk usage, etc...
By avoiding increasing complexity from scope-creep, Embark will remain crisp and friendly :)

##Usage

You can add the [Embark NuGet package](https://www.nuget.org/packages/Embark/), copy paste the sample code and then happily just continue developing.

If you want to save data over a network:
```csharp
// start a new server
var server = new Embark.Server();
server.Start();
```
or you can download a simple server from [here](https://trello-attachments.s3.amazonaws.com/54f89f538ec1e186a911c534/5527fc8a8a55d94cbed0ab17/c3e0c011826d1fe4519a46f07e46b97e/BasicServer.zip). 
*NOTE: For the WCF server to work, either run it in admin mode or give access rights to the [your-machine:port/embark/](http://stackoverflow.com/a/17242260/4650900) uri  namespace.*

##Development

To check out current developments go to [embarkDB on trello](https://trello.com/b/rtqlPmrM/development).

Feel free to contact embarkDB@gmail.com with any feedback, suggestions or to get involved!
