using Neo4j.NDP.CSharpDriver.Logging;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Net.Sockets;
using System.Collections.Generic;


namespace Neo4j.NDP.CSharpDriver
{
    internal class Connection : IConnection
    {
        private const string initText = "ExampleDriver/1.0";

        private readonly TcpClient client;
        private readonly ChunkStream chunkStream;
        private readonly ILogger logger;

        public Connection(TcpClient client, NetworkStream stream, ILogger logger)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (stream == null) throw new ArgumentNullException("stream");

            this.client = client;
            this.chunkStream = new ChunkStream(stream);
            this.logger = logger;

            logger.Info("NDPv1 connection established!");

            bool initialized = Initialize();
            if (!initialized)
            {
                throw new InvalidOperationException("Failed to initialize the connection");
            }

            logger.Info("Initialization was successful");
        }

        public IEnumerable<IEntity> Run(string statement, IDictionary<string, object> parameters = null)
        {
            logger.Info("Running statement: {0}", statement);

            IMessageMap parametersMessage = BuildParameters(parameters);

            IMessageStructure runRequest = new MessageStructure(
                StructureSignature.Run, new IMessageObject[] {
					new MessageText(statement),
                    parametersMessage 
		    });

            chunkStream.Write(runRequest);

            IMessageStructure pullAllRequest = new MessageStructure(
                StructureSignature.PullAll
            );
            chunkStream.Write(pullAllRequest);

            IMessageObject runResponse = chunkStream.Read();
            if (runResponse == null || runResponse.Type != MessageObjectType.Structure)
            {
                throw new InvalidOperationException("Unexpected data received: " + runResponse ?? "");
            }

            IMessageStructure runResponseStructure = runResponse as IMessageStructure;
            if (runResponseStructure.Signature != StructureSignature.Success)
            {
                throw new InvalidOperationException("Run request failed with: " + runResponseStructure.ToString());
            }

            logger.Info("Statement ran with success");

            IEntityBuilder graphBuilder = new EntityBuilder(); // TODO: Inject this or use factory
            while (true)
            {
                IMessageObject result = chunkStream.Read();
                logger.Info("Received message: {0}", result != null ? result.ToString() : "hmm");

                if (result.IsStructureWithSignature(StructureSignature.Record))
                {
                    IEnumerable<IEntity> entities = graphBuilder.BuildFromRecord((IMessageStructure)result);
                    foreach (IEntity entity in entities)
                    {
                        yield return entity;
                    }
                }
                else if (result.IsStructureWithSignature(StructureSignature.Success)) 
                {
                    break;
                }
                else if (result.IsStructureWithSignature(StructureSignature.Failure))
                {
                    // TODO: Ack failure
                    break;
                }
                else 
                {
                    throw new InvalidOperationException(string.Format("Unexpected response: {0}", result));
                }
            }

            logger.Info("Finished with the run");
           
            yield break;
        }

        private IMessageMap BuildParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null) 
            {
                return new MessageMap();
            }

            IDictionary<IMessageObject, IMessageObject> maps = new Dictionary<IMessageObject, IMessageObject>();

            foreach (var kvp in parameters)
            {
                IMessageObject key = new MessageText(kvp.Key);

                IMessageObject value;
                if (kvp.Value == null)
                {
                    value = new MessageNull();
                }
                else if (kvp.Value.GetType() == typeof(int))
                {
                    value = new MessageInt((int)kvp.Value);
                }
                else if (kvp.Value.GetType() == typeof(string))
                {
                    value = new MessageText((string)kvp.Value);
                }
                else 
                {
                    throw new NotImplementedException();
                }

                maps.Add(key, value);
            }
            
            return new MessageMap(maps);
        }

        public void Dispose()
        {
            logger.Info("Shutting down and closing connection");
            chunkStream.Dispose();
            ((IDisposable)client).Dispose();
        }


        private bool Initialize()
        {
            logger.Info("Initializing connection");

            IMessageStructure initRequest = new MessageStructure(
                StructureSignature.Init,
                new MessageText(initText)
            );

            chunkStream.Write(initRequest);
            IMessageStructure initResponse = chunkStream.Read() as IMessageStructure;
            if (initResponse == null) return false;
            if (initResponse.Signature != StructureSignature.Success) return false;

            return true;
        }
    }
}
