using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.zmq;
using NetMQ.Sockets;

namespace GSharp.Events
{
  public class Publisher
  {
    private readonly NetMQContext _context;

    private string _address;
    private PublisherSocket _socket;

    public Publisher(NetMQContext context, string address)
    {
      _context = context;
      _socket = _context.CreatePublisherSocket();
      _address = address;
      _socket.Bind(_address);
    }

    public void FireEvent(string type)
    {
      _socket.Send(type, true);
    }

    public void FireEvent(string type, byte[] data)
    {
      _socket.SendMore(type, true);
      _socket.Send(data, data.Length, true);
    }
  }
}
