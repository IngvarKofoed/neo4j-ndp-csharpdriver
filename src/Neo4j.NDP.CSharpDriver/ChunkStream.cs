using Neo4j.NDP.CSharpDriver.Extensions;
using Neo4j.NDP.CSharpDriver.Logging;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Neo4j.NDP.CSharpDriver
{
    /// <summary>
    /// A <see cref="ChunkStream"/> is a stream that reads and writes
    /// <see cref="IMessageObjects"/> instances and handles the chunking, flushing
    /// and terminating of message- 
    /// </summary>
    public class ChunkStream : IDisposable
    {
        private static readonly byte[] EndOfMessage = new byte[] { 0, 0 };

        private readonly Stream stream;
        private readonly ILogger logger;

        // TODO: Inject his
        private readonly IBitConverter bitConverter = new BigEndianTargetBitConverter();
        // TODO: Inject his
        private readonly MessageObjectSerializer serializer = new MessageObjectSerializer(
            new PackSteamBuilderFactory(new BigEndianTargetBitConverter()));
        // TODO: Inject his
        private readonly MessageObjectDeserializer deserializer = 
            new MessageObjectDeserializer(new PackStreamUnpacker(new BigEndianTargetBitConverter()));

        public ChunkStream(Stream stream, ILogger logger = null)
        {
            this.stream = stream;
            this.logger = logger;
        }

        public IMessageObject Read()
        {
            logger.Debug("Receiving header");

            byte[] chunkSizeData = new byte[2];

            stream.Read(chunkSizeData, 0, 2);
            short chunkSize = bitConverter.ToInt16(chunkSizeData);
            if (chunkSize == 0) throw new InvalidOperationException("Unexpected chunk size of size zero received");

            logger.Debug("Received chunk header {0} ({1})", chunkSizeData.ToReadableString(), chunkSize);

            byte[] chunkData = new byte[chunkSize];
            stream.Read(chunkData, 0, chunkSize);
            logger.Debug("Received chunk {0}", chunkData.ToReadableString());

            IMessageObject message = deserializer.Deserialize(chunkData);
            logger.Debug("Received message {0}", message.ToString());

            stream.Read(chunkSizeData, 0, 2);
            chunkSize = bitConverter.ToInt16(chunkSizeData);
            if (chunkSize != 0) throw new InvalidOperationException("Missing termination header");

            return message;
        }

        public void Write(IMessageObject messageObject)
        {
            logger.Debug("Sending message: {0}", messageObject.ToString());

            byte[] bytes = serializer.Serialize(messageObject);
            byte[] headerBytes = bitConverter.GetBytes((short)bytes.Length);

            logger.Debug("Sinding bytes: ({0}){1}", headerBytes.ToReadableString(), bytes.ToReadableString());

            stream.Write(headerBytes);
            stream.Write(bytes);
            Flush();
        }

        public void Write(IEnumerable<IMessageObject> messageObjects)
        {
            foreach (IMessageObject messageObject in messageObjects)
            {
                Write(messageObject);
            }
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        private void Flush()
        {
            logger.Debug("Sinding bytes: {0}", EndOfMessage.ToReadableString());
            stream.Write(EndOfMessage);
        }
    }
}
