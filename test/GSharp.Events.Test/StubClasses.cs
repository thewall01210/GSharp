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

    public void SetData(byte[] data)
    {
      Data = data;
    }
  }
}
