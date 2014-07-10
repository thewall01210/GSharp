using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using NUnit;
using NUnit.Framework;
using NetMQ;

namespace GSharp.Events.Test
{
  public class PublisherStub : Publisher
  {
    public PublisherStub(NetMQContext context, string address)
      : base(context, address)
    {
    }
  }

  public class SubscriberStub : Subscriber
  {
    public byte[] Data { get; set; }

    public SubscriberStub(NetMQContext context, string address)
      : base(context, address)
    {

    }

    public void Handle(byte[] data)
    {
      Data = data;
    }
  }

  [TestFixture]
  public class PublishSubscribeTest
  {
    PublisherStub publisher;
    SubscriberStub subscriber;

    [Test]
    public void CanConnect()
    {
      // var address = string.Format("{0}://{1}:{2}", "tcp", IPAddress.IPv6Loopback, 5556);
      var address = string.Format("{0}://{1}:{2}", "tcp", IPAddress.Loopback, 5556);

      var pubContext = NetMQContext.Create();
      var subContext = NetMQContext.Create();
      publisher = new PublisherStub(pubContext, address);
      subscriber = new SubscriberStub(subContext, address);

      subscriber.AddListener("eventType", subscriber.Handle);
      Thread.Sleep(100);
      publisher.FireEvent("eventType", new byte[] { 1, 0, 0, 0, 1 });

      Assert.NotNull(subscriber.Data);
    }
  }
}
