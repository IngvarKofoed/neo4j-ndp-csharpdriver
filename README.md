# neo4j-ndp-csharpdriver

## Information
This is heavily base on: https://github.com/nigelsmall/ndp-howto 

This is very much a work in progress, so stay tuned!

## Gettings started
Read more on getting started here: https://github.com/nigelsmall/ndp-howto 

## Hello world
```C#
Neo4jService neo4jService = new Neo4jService();
using (IConnection connection = neo4jService.CreateConnection("localhost", 7687))
{
    connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Ingvar'})").ToList();
    connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Kofoed'})").ToList();
    connection.Run("CREATE (a:Person {name:'Kofoed'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Jensen'})").ToList();
    connection.Run("CREATE (a:Person {name:'Jensen'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Martin'})").ToList(); 

    IEnumerable<IEntity> entities = connection.Run("MATCH (n)-[r]->(c) RETURN n, r");

    Console.WriteLine("Received entities: ");
    foreach (IEntity entity in entities)
    {
        if (entity.EntityType == EntityType.Node)
        {
            INode node = entity as INode;
            Console.WriteLine(node); 
        }
        else if (entity.EntityType == EntityType.Relationship)
        {
            IRelationship relationship = entity as IRelationship;
            Console.WriteLine(relationship); 
        }
    }
}
```
#### Output from Hello World
```
Received entities: 
Node: node/0 [Person] {name:Martin}
Relationship: rel/0, node/0-KNOWS->node/1 {from:Home}
Node: node/2 [Person] {name:Martin}
Relationship: rel/1, node/2-KNOWS->node/3 {from:Home}
Node: node/4 [Person] {name:Kofoed}
Relationship: rel/2, node/4-KNOWS->node/5 {from:Home}
Node: node/6 [Person] {name:Jensen}
Relationship: rel/3, node/6-KNOWS->node/7 {from:Home}
```

## TODO list
* Parameters support when running queries
* Support for Paths
* Support of other return values than node and relationships (see next point)
* Better generic typed way of getting the results of a Run
* More unittests
* More documentation
* And lits of minor things
