using System;
using SlimDX;

namespace GSharp.Graphics
{
  public class Model : IDisposable
  {
    public static int Vector3Size = 12; // HAX!!! "sizeof(Vector3)" would need to be marked as unsafe

    public Model(Vector3[] vertices, Vector3[] colors)
    {
      Vertices = vertices;
      Colors = colors;
      BufferSize = Vertices.Length * Vector3Size + Colors.Length * Vector3Size;
      DataStream = new DataStream(BufferSize, true, true);

      foreach (var vertex in Vertices)
      {
        DataStream.Write<Vector3>(vertex);
      }

      foreach (var color in Colors)
      {
        DataStream.Write<Vector3>(color);
      }

      DataStream.Position = 0;
    }

    public Vector3[] Vertices { get; private set; }

    public Vector3[] Colors { get; private set; }

    public int BufferSize { get; private set; }

    public DataStream DataStream { get; private set; }

    public void Dispose()
    {
      DataStream.Close();
    }
  }
}
