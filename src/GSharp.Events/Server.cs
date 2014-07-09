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
  public class Server
  {
    private readonly NetMQContext _context;

    private readonly string _id;

    public Server(NetMQContext context, string id)
    {
      _context = context;
      _id = id;
    }

    public Task Start(CancellationToken token, string address)
    {
      var ready = new TaskCompletionSource<bool>();

      Task.Factory.StartNew(() => StartNewConnection(token, address, ready), token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

      return ready.Task;
    }

    private void StartNewConnection(CancellationToken token, string address, TaskCompletionSource<bool> ready)
    {
      using (var socket = _context.CreateResponseSocket())
      {
        socket.Bind(address);
        ready.SetResult(true);
        byte[] bytes;

        while (!token.IsCancellationRequested)
        {
          bytes = socket.Receive(SendReceiveOptions.None);

          Console.WriteLine(Encoding.ASCII.GetString(bytes));
        }
      }
    }
  }
}
