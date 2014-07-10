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
  public class Subscriber
  {
    private NetMQContext _context;
    private string _address;
    public delegate void Callback(byte[] data);
    public Callback _callback;

    public Subscriber(NetMQContext context, string address)
    {
      _context = context;
      _address = address;
    }

    public void AddListener(string type, Callback callback)
    {
      _callback = callback;
      using (var socket = _context.CreateSubscriberSocket())
      {
        socket.Connect(_address);
        socket.Subscribe(type);
      }
    }
  }
}
