using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NUnit;
using NUnit.Framework;
using NetMQ;

namespace GSharp.Events.Test
{
  [TestFixture]
  class PublishSubscribeTest
  {
    Client client;
    Server server;

    [Test]
    void CanConnect()
    {
      var address = "tcp://127.0.0.1:9988";

      var serverContext = NetMQContext.Create();
      server = new Server(serverContext, "Test Server");
      var serverToken = new CancellationTokenSource();
      server.Start(serverToken.Token, address);


      var clientContext = NetMQContext.Create();
      client = new Client(clientContext, "Test Server");
      var clientToken = new CancellationTokenSource();
      client.Start(clientToken.Token, address);

      clientToken.Cancel();
      clientContext.Dispose();
      serverToken.Cancel();
      serverContext.Dispose();
    }
  }
}
