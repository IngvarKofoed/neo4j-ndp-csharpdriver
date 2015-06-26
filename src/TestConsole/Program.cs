using System;
using System.Linq;
using Neo4j.NDP.CSharpDriver;
using Neo4j.NDP.CSharpDriver.Extensions;
using Neo4j.NDP.CSharpDriver.Logging;
using System.Collections.Generic;
using System.Text;

namespace TestConsole
{
    public class ConsoleLogger : IInternalLogger
    {
        private readonly LogSeverity minimumSeverity;


        public ConsoleLogger(LogSeverity minimumSeverity = LogSeverity.Warning)
        {
            this.minimumSeverity = minimumSeverity; 
        }

        public void Write(LogSeverity severity, string format, params object[] arguments)
        {
            if (severity < minimumSeverity) return;

            ConsoleColor oldColor = Console.ForegroundColor;

            string prefix = "";
            if (severity == LogSeverity.Debug) 
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                prefix = "DEBUG";
            }
            else if (severity == LogSeverity.Information) 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                prefix = "INFO ";
            }
            else if (severity == LogSeverity.Warning) 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                prefix = "WARN ";
            }
            else if (severity == LogSeverity.Error) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                prefix = "ERROR";
            }
            else if (severity == LogSeverity.Fatal) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                prefix = "FATAL";
            }

            Console.WriteLine(prefix + ": " + string.Format(format, arguments));

            Console.ForegroundColor = oldColor;
        }
    }
    
    class MainClass
    {
        public static void Main(string[] args)
        {
            Neo4jService neo4jService = new Neo4jService(new ConsoleLogger());
            using (IConnection connection = neo4jService.CreateConnection("localhost", 7687))
            {
                //connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Ingvar'})").ToList();
                //connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Kofoed'})").ToList();
                //connection.Run("CREATE (a:Person {name:'Kofoed'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Jensen'})").ToList();
                //connection.Run("CREATE (a:Person {name:'Jensen'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Martin'})").ToList(); 

                //IEnumerable<IEntity> entities = connection.Run("MATCH (n)-[r]->(c) RETURN n, r");


                var records = connection.Run<Tuple<INode, string>>("MATCH (n) RETURN n, n.name").ToList();
                 
                Console.WriteLine("Results: ");
                foreach (var record in records)
                {
                    Console.WriteLine("n = {0}, n.name = {1}", record.Item1, record.Item2);
                }
            }

            Console.WriteLine("All done!");
        }

        /*

        public static void Main_Old(string[] args)
        {
            Neo4jService neo4jService = new Neo4jService(new ConsoleLogger());
            using (IConnection connection = neo4jService.CreateConnection("localhost", 7687))
            {
                connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Ingvar'})");
                connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Kofoed'})");
                connection.Run("CREATE (a:Person {name:'Kofoed'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Jensen'})");
                connection.Run("CREATE (a:Person {name:'Jensen'})-[ab:KNOWS {from:'Home'}]->(b:Person {name:'Martin'})");

                //connection.Run("CREATE (a:Person {name:'Martin'})-[ab:KNOWS]->(b:Person {name:'Kurt'}) RETURN a, b");
                //connection.Run("MATCH (a:Person) RETURN a.name AS name, a.age AS age");

                IEnumerable<IEntity> entities = connection.Run("MATCH (n)-[r]->(c) RETURN n, r");
                //connection.Run("MATCH (n)-[r]->(c) WHERE n.name='Martin' RETURN n, r");

                Console.WriteLine("Received entities: ");
                foreach (IEntity entity in entities)
                {
                    if (entity.EntityType == EntityType.Node)
                    {
                        INode node = entity as INode;
                        Console.WriteLine("{0}: {1}", node.EntityType, node.Id); // Node.ToString
                    }
                    else if (entity.EntityType == EntityType.Relationship)
                    {
                        IRelationship relationship = entity as IRelationship;
                        Console.WriteLine("{0}: {1}, {2}-{3}->{4}", 
                            relationship.EntityType, relationship.Id, 
                            relationship.StartNodeId, relationship.Type, 
                            relationship.EndNodeId); // Relationship.ToString
                    }
                }
            }

            Console.WriteLine("All done!");
        }*/
    }

    /*
     * The map result on success?
     * 
     */
}
