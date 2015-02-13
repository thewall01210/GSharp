using System;
using SlimDX;

namespace GSharp.Graphics
{
  public class Model : IDisposable
  {
    public static int Vector3Size = 12; // HAX!!! "sizeof(Vector3)" would need to be marked as unsafe

    public Model(Vector3[] vertices)
    {
      Vertices = vertices;
      BufferSize = Vertices.Length * Vector3Size;
      DataStream = new DataStream(Vertices, true, true);
      DataStream.Position = 0;
    }

    public Vector3[] Vertices { get; private set; }

    public int BufferSize { get; private set; }

    public DataStream DataStream { get; private set; }

    private Vector3[] colors;

    public void AddColors(Vector3[] colorArray)
    {
      BufferSize += colorArray.Length * Vector3Size;
      colors = colorArray;

      foreach (var color in colors)
      {
        DataStream.Write(color);
      }
    }

    public void Dispose()
    {
      DataStream.Close();
    }
  }
}
