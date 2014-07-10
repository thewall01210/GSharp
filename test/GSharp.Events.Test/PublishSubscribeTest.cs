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
  public class PublishSubscribeTest
  {
    Publisher publisher;
    Server server;

    [Test]
    public void CanConnect()
    {
      var address = "tcp://127.0.0.1:9988";

      var pubContext = NetMQContext.Create();
      publisher = new Publisher(pubContext, address);
      


      Assert.True(true);
    }
  }
}
