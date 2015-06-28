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
    connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Ingvar'})");
    connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Kofoed'})");
    connection.Run("CREATE (a:Person {name:'Kofoed'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Jensen'})");
    connection.Run("CREATE (a:Person {name:'Jensen'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Martin'})"); 

    // Returning nodes
    var nodes = connection.Run<INode>("MATCH (n)-[r]->(c) RETURN n").ToList();                 
    Console.WriteLine("Results (Returning nodes): ");
    foreach (INode node in nodes)
    {
        Console.WriteLine("n = {0}", node);
    }

    // Returning property values
    var names = connection.Run<string>("MATCH (n)-[r]->(c) RETURN n.name").ToList();                 
    Console.WriteLine("Results (Returning property values): ");
    foreach (string name in names)
    {
        Console.WriteLine("n.name = {0}", name);
    }

    // Returning composite value
    var records = connection.Run<Tuple<INode, IRelationship, string>>("MATCH (n)-[r]->(c) RETURN n, r, n.name").ToList();                 
    Console.WriteLine("Results (Returning composite values): ");
    foreach (var record in records)
    {
        Console.WriteLine("n = {0}, r = {1}, n.name = {2}", record.Item1, record.Item2, record.Item3);
    }
}
```
#### Output from Hello World
```
Results (Returning nodes): 
n = Node: node/0 [Person] {name:Martin}
n = Node: node/2 [Person] {name:Martin}
n = Node: node/4 [Person] {name:Kofoed}
n = Node: node/6 [Person] {name:Jensen}
Results (Returning property values): 
n.name = Martin
n.name = Martin
n.name = Kofoed
n.name = Jensen
Results (Returning composite values): 
n = Node: node/0 [Person] {name:Martin}, r = Relationship: rel/0, node/0-KNOWS->node/1 {from:Home}, n.name = Martin
n = Node: node/2 [Person] {name:Martin}, r = Relationship: rel/1, node/2-KNOWS->node/3 {from:Home}, n.name = Martin
n = Node: node/4 [Person] {name:Kofoed}, r = Relationship: rel/2, node/4-KNOWS->node/5 {from:Home}, n.name = Kofoed
n = Node: node/6 [Person] {name:Jensen}, r = Relationship: rel/3, node/6-KNOWS->node/7 {from:Home}, n.name = Jensen

```

## TODO list
* Support for Paths
* Handling failures from server
* More unittests
* More documentation
* And lots of minor things
