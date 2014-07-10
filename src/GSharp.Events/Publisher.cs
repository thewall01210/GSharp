using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.zmq;

namespace GSharp.Events
{
  public class Publisher
  {
    private readonly NetMQContext _context;

    private string _address;

    public Publisher(NetMQContext context, string address)
    {
      _context = context;
      _address = address;
    }

    public void FireEvent(string type)
    {
      using (var socket = _context.CreatePublisherSocket())
      {
        socket.Bind(_address);
        socket.SendMore(type);
      }
    }

    public void FireEvent(string type, byte[] data)
    {
      using (var socket = _context.CreatePublisherSocket())
      {
        socket.Bind(_address);
        socket.SendMore(type);
        socket.Send(data);
      }
    }

  }
}
