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
  public class Subscriber : IDisposable
  {
    private NetMQContext _context;
    private string _address;
    private string _type;
    private Task Poller;
    private Action<byte[]> _callback;

    public Subscriber(NetMQContext context, string address)
    {
      _context = context;
      _address = address;
    }

    public void AddListener(string type, Action<byte[]> callback)
    {
      _type = type;
      _callback = callback;
      using (var socket = _context.CreateSubscriberSocket())
      {
        socket.Connect(_address);
        socket.Subscribe(type);
      }
      Poller = new Task(Poll);
      Poller.Start();
    }

    public void Dispose()
    {
      Poller.Dispose();
    }

    private void Poll()
    {
      using (var socket = _context.CreateSubscriberSocket())
      {
        while (true)
        {
          bool more;
          var type = socket.ReceiveString(SendReceiveOptions.DontWait, out more);
          if (type == _type)
          {
            if (more)
            {
              _callback.Invoke(socket.Receive(SendReceiveOptions.DontWait));
            }
          }
        }
      }
    }
  }
}
