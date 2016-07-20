# Easy-Doc-Db

![Build Status]
(https://mocoding.visualstudio.com/_apis/public/build/definitions/6a316467-5a7a-41a0-98fb-959a5b880ab1/21/badge)

easy-doc-db - is a simple yet extendable .NET Core library that allows having embedded document storage. The lib allows working with documents in two modes - single document and collection of documents. It is best suited for small or medium-sized project with limited or predictably small amount of data.

Key benefits:
 - Read optimized
 - No data and schema migrations
 - Extandable
 - Thread-safe

Demo:

```cs
IRepository repository = new Repository(new JsonSerializer(); 
var users = await repository.InitCollection<User>("../data/users");

// read all
var allDocs = users.Documents.Select(u => u.Data);

//...
// create new
var newUser = users.New();
newUser.Data.FirstName = "Name1";
newUser.Data.LastName = "LastName1";
await newUser.Save();

//...
//delete
await newUser.Delete();

```

"../data/users" – path for storing collection of documents. 
Each document will be saved as a plain json file. XML and YAML are also supported.

Please see more demo in examples folder.

### Supported platforms

We support next platforms:

- .NET 4.5.1
- .NET Core 1.0

## Documentation

IRepository supports working with a single document or collection of documents.

```cs
Task<IDocument<T>> Init<T>(string conn) where T : class, new();
```
 - will load or create a new document at the location specified by input parameter.

```cs
Task<IDocumentCollection<T>> InitCollection<T>(string conn) where T : class, new();
```
- will create new folder at the location specified by input parameter. All files will go there using the following format: {guid}.{format}.


IDocumentCollection provides access to all documents and allows creating a new one.

```cs
ImmutableArray<IDocument<T>> Documents { get; }
IDocument<T> New();
```

IDocument allows saving, deleting and updating the document in a thread safe manner.
  
 ```cs
T Data { get; }
Task SyncUpdate(Action<T> data);
Task Save();
Task Delete();
```
NOTE: Data property is not thread safe.

## Extend
 
### Serializers

There are currently 3 serializers availible: Json, Yaml and Xml. They are availible via their nugets:
- Mocoding.EasyDocDb.Json
- Mocoding.EasyDocDb.Yaml
- Mocoding.EasyDocDb.Xml

For custom serializer you need to implement interface IDocumentSerializer and pass to Repository constructor.


```cs
public interface IDocumentSerializer
{
    string Type { get; }
    string Serialize<T>(T data) where T : class, new();
    T Deserialize<T>(string content) where T : class, new();
}
```

### Storage

By default documents are embedded and stored to one of your local folders. The default behavior could be overridden by custom implementation of IDocumentStorage interface.
Below we will show you methods for reading, writing, deleting and other operations with document.

```cs
public interface IDocumentStorage
{
   Task<string> Read(string @ref);
   Task Write(string @ref, string document);
   Task Delete(string @ref);
   Task<string[]> Enumerate(string collectionRef);
   string NewRef(string collectionRef, string name);
}
```

