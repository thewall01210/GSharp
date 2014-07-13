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
  public class Subscriber : IDisposable
  {
    private string _address;
    private NetMQContext _context;
    private string _type;
    private SubscriberSocket _socket;
    private Task Poller;
    private Action<byte[]> _callback;

    public Subscriber(NetMQContext context, string address)
    {
      _address = address;
      _context = context;
      _socket = _context.CreateSubscriberSocket();
      _socket.Connect(_address);
      Poller = new Task(Poll);
    }

    public void AddListener(string type, Action<byte[]> callback)
    {
      _type = type;
      _callback = callback;
      _socket.Subscribe(_type);
      Poller.Start();
    }

    public void Dispose()
    {
      Poller.Dispose();
    }

    private  void Poll()
    {
      while (true)
      {
        try
        {
          bool more;
          // fail...
          string type = _socket.ReceiveString(SendReceiveOptions.None, out more);
          if (type == _type)
          {
            if (more)
            {
              _callback.Invoke(_socket.Receive(SendReceiveOptions.None));
            }
          }
        }
        catch (AgainException againException)
        {
        }
      }
    }
  }
}
