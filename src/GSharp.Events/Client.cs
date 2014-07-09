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
  public class Client
  {
    private readonly NetMQContext _context;

    private readonly string _id;

    public Client(NetMQContext context, string id)
    {
      _context = context;
      _id = id;
    }

    public Task Start(CancellationToken token, string address)
    {
      var ready = new TaskCompletionSource<bool>();

      Task.Factory.StartNew(
        () => StartNewConnection(token, address, ready),
        token, TaskCreationOptions.LongRunning,
        TaskScheduler.Default);

      return ready.Task;
    }

    private void StartNewConnection(CancellationToken token, string address, TaskCompletionSource<bool> ready)
    {
      using (var socket = _context.CreateRequestSocket())
      {
        socket.Connect(address);
        ready.SetResult(true);

        while (!token.IsCancellationRequested)
        {
          var instantMessage = Console.ReadLine();
          if (!string.IsNullOrEmpty(instantMessage))
          {
            socket.Send(Encoding.ASCII.GetBytes(string.Format("Server: {0}", instantMessage)));
          }
        }
      }
    }
  }
}
