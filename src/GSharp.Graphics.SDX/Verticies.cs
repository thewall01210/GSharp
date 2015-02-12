using System;
using SlimDX;

namespace GSharp.Graphics.SDX
{
  public class Verticies : IDisposable
  {
    private const int Vector3Size = 12; // SUBSTITUE  "sizeof(Vector3)" would need to be marked as unsafe

    public Verticies(Vector3[] vertices)
    {
      Vertices = vertices;
      BufferSize = Vertices.Length * Vector3Size;
      DataStream = new DataStream(Vertices, true, true);
      DataStream.Position = 0;
    }

    public Vector3[] Vertices { get; private set; }

    public int BufferSize { get; private set; }

    public DataStream DataStream { get; private set; }

    public void Dispose()
    {
      DataStream.Close();
    }
  }
}
