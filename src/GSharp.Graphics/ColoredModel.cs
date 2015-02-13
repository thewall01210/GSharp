using System;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;

namespace GSharp.Graphics
{
  public class ColoredModel : IDisposable
  {
    public static int VertexSize = 12; // HAX!!! "sizeof(Vector3)" would need to be marked as unsafe

    public ColoredModel(Vector3[] vertices, Vector3[] colors)
    {
      Vertices = vertices;
      Colors = colors;
      BufferSize = Vertices.Length * VertexSize + Colors.Length * VertexSize;
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

    public InputElement[] GetSharderInputElements()
    {
      return new[]
      {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
        new InputElement("COLOR", 0, Format.R32G32B32_Float, ColoredModel.VertexSize * Vertices.Length, 0)
      };
    }

    public void Dispose()
    {
      DataStream.Close();
    }
  }
}
