# neo4j-ndp-csharpdriver

## Information
This is heavily base on: https://github.com/nigelsmall/ndp-howto 

This is very much a work in progress, so stay tuned!

## Gettings started
Read more on getting started here: https://github.com/nigelsmall/ndp-howto 

## Hello world
```C#
Neo4jService neo4jService = new Neo4jService(new ConsoleLogger());
using (IConnection connection = neo4jService.CreateConnection("localhost", 7687))
{
    connection.Run("CREATE (a:Person {name:'Alice'})-[ab:KNOWS]->(b:Person {name:'Bob'}) RETURN a, b");
    connection.Run("MATCH (a:Person) RETURN a.name AS name, a.age AS age");
}

Console.WriteLine("All done!");
Console.ReadKey();
```
#### Output from Hello World
```
INFO : Connecting to localhost:7687
INFO : Connected to localhost:7687
INFO : Supported protocol versions are: 1, 0, 0, 0
INFO : Protocol version 1 agreed
INFO : NDPv1 connection established!
INFO : Initializing connection
INFO : Received message Success{ (  ) }
INFO : Initialization was successful
INFO : Running statement: CREATE (a:Person {name:'Alice'})-[ab:KNOWS]->(b:Person {name:'Bob'}) RETURN a, b
INFO : Received message Success{ ( "fields"->["a", "b"] ) }
INFO : Statement ran with success
INFO : Received message Record{ [78{ "node/0", ["Person"], ( "name"->"Alice" ) }, 78{ "node/1", ["Person"], ( "name"->"Bob" ) }] }
INFO : Running statement: MATCH (a:Person) RETURN a.name AS name, a.age AS age
INFO : Received message Success{ (  ) }
INFO : Statement ran with success
INFO : Received message Success{ ( "fields"->["name", "age"] ) }
INFO : Shutting down and closing connection

```
