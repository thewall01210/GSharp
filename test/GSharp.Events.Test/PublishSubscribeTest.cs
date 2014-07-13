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

      var context = NetMQContext.Create();
      publisher = new PublisherStub(context, address);
      subscriber = new SubscriberStub(context, address);

      subscriber.AddListener("a", subscriber.Handle);
      Thread.Sleep(1000);
      publisher.FireEvent("a", new byte[] { 0 });

      Thread.Sleep(1000);
      Assert.NotNull(subscriber.Data);
    }
    
    [Test]
    public void TopicPubSub()
    {
      using (NetMQContext contex = NetMQContext.Create())
      {
        using (var pub = contex.CreatePublisherSocket())
        {
          pub.Bind("tcp://127.0.0.1:5002");

          using (var sub = contex.CreateSubscriberSocket())
          {
            sub.Connect("tcp://127.0.0.1:5002");
            sub.Subscribe("A");

            // let the subscrbier connect to the publisher before sending a message
            Thread.Sleep(500);

            pub.SendMore("A");
            pub.Send("Hello");

            bool more;

            string m = sub.ReceiveString(out more);

            Assert.AreEqual("A", m);
            Assert.IsTrue(more);

            string m2 = sub.ReceiveString(out more);

            Assert.AreEqual("Hello", m2);
            Assert.False(more);
          }
        }
      }
    }
  }
}
