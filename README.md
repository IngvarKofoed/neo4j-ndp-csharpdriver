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
    connection.Run("CREATE (a:Person {name:'Alice1'})-[ab:KNOWS]->(b:Person {name:'Bob1'}) RETURN a, b");
    connection.Run("MATCH (a:Person) RETURN a.name AS name, a.age AS age");
}

Console.WriteLine("All done!");
Console.ReadKey();
```
