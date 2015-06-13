using Neo4j.NDP.CSharpDriver.Extensions;
using Neo4j.NDP.CSharpDriver.Logging;
using Neo4j.NDP.CSharpDriver.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Sockets;


namespace Neo4j.NDP.CSharpDriver
{
    public class Neo4jService
    {
        private readonly ILogger logger;
        IBitConverter bitConverter = new BigEndianTargetBitConverter();
        // TODO: This should be injected

        public Neo4jService(ILogger logger = null)
        {
            this.logger = logger;
        }

        public IConnection CreateConnection(string host, int portNumber)
        {
            logger.Info("Connecting to {0}:{1}", host, portNumber);
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = new TcpClient(host, portNumber);
                stream = client.GetStream();
                logger.Info("Connected to {0}:{1}", host, portNumber);

                int agreedVersion = DoHandshake(stream);
                if (agreedVersion == 1)
                {
                    logger.Info("Protocol version {0} agreed", agreedVersion);
                    IConnection connection = new Connection(client, stream, logger);
                    return connection;
                }
                else
                {
                    logger.Warn("Closing connection as no protocol version could be agreed");
                    Cleanup(client, stream);
                    return null;
                }

            }
            catch (Exception ex) // TODO: Find possible connections
            {
                logger.Info("Shutting down and closing connection due to the error: {0}", ex.Message);
                Cleanup(client, stream);
                throw;
            }

        }

        private void Cleanup(TcpClient client, NetworkStream stream)
        {
            if (stream != null)
            {
                stream.Dispose();
            }

            if (client != null)
            {
                ((IDisposable)client).Dispose();
            }
        }

        private int DoHandshake(NetworkStream stream)
        {
            // Send details of the protocol versions supported
            int[] supportedVersion = new int[] { 1, 0, 0, 0 };
            // Are there more than one supported version, and how are this send?
            logger.Info("Supported protocol versions are: " + supportedVersion.ToReadableString());
            byte[] data = PackVersions(supportedVersion);
            logger.Debug("Sending handshake: " + data.ToReadableString());
            stream.Write(data);

            // Handle the handshake response
            data = new byte[4];
            stream.Read(data, 0, 4);
            logger.Debug("Received handshake data: " + data.ToReadableString());
            int agreedVersion = GetAgreedVersion(data);

            return agreedVersion;
        }

        private byte[] PackVersions(IEnumerable<int> versions)
        {
            List<byte> bytes = new List<byte>();
            foreach (int version in versions)
            {
                bytes.AddRange(bitConverter.GetBytes(version));
            }
            return bytes.ToArray();
        }

        private int GetAgreedVersion(byte[] data)
        {
            return bitConverter.ToInt32(data);
        }
    }
}
