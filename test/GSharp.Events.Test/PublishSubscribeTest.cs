using System;
using System.Collections.Generic;
using System.Linq;
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
      var address = "tcp://127.0.0.1:9988";

      var context = NetMQContext.Create();
      publisher = new PublisherStub(context, address);
      subscriber = new SubscriberStub(context, address);

      subscriber.AddListener("eventType", subscriber.Handle);

      publisher.FireEvent("eventType", new byte[] { 1, 0, 0, 0, 1 });

      Thread.Sleep(10000);

      Assert.AreEqual(5, subscriber.Data.Length);
    }
  }
}
