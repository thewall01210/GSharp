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
  [TestFixture]
  public class PublishSubscribeTest
  {
    PublisherStub _publisher;
    SubscriberStub _subscriber;

    [Test]
    public void DataNotNullWithSameContext()
    {
      var address = string.Format("{0}://{1}:{2}", "tcp", IPAddress.Loopback, 5556);
      var context = NetMQContext.Create();
      _publisher = new PublisherStub(context, address);
      _subscriber = new SubscriberStub(context, address);

      _subscriber.AddListener("a", _subscriber.SetData);
      Thread.Sleep(100);
      _publisher.FireEvent("a", new byte[] { 0 });

      Thread.Sleep(100);
      Assert.NotNull(_subscriber.Data);
    }

    [Test]
    public void DataNotNullWithDifferentContexts()
    {
      var address = string.Format("{0}://{1}:{2}", "tcp", IPAddress.Loopback, 5557);
      var context1 = NetMQContext.Create();
      var context2 = NetMQContext.Create();
      _publisher = new PublisherStub(context1, address);
      _subscriber = new SubscriberStub(context2, address);

      _subscriber.AddListener("a", _subscriber.SetData);
      Thread.Sleep(100);
      _publisher.FireEvent("a", new byte[] { 0 });

      Thread.Sleep(100);
      Assert.NotNull(_subscriber.Data);
    }
    
    [Test]
    public void TopicPubSub()
    {
      var address = string.Format("{0}://{1}:{2}", "tcp", IPAddress.Loopback, 5558);
      using (NetMQContext contex = NetMQContext.Create())
      {
        using (var pub = contex.CreatePublisherSocket())
        {
          pub.Bind(address);

          using (var sub = contex.CreateSubscriberSocket())
          {
            sub.Connect(address);
            sub.Subscribe("A");

            // let the subscriber connect to the publisher before sending a message
            Thread.Sleep(100);

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
