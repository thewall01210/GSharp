using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;
using GSharp.Graphics;

namespace GSharp.Content
{
    public static class ContentLoader
    {
      public static Texture2D GetTexture(Device device, string fileName)
      {
        return Texture2D.FromFile(device, fileName);
      }

      public static ColoredModel GetColoredBox(Vector3 color)
      {
        return new ColoredModel(
        new[]
          {
            new Vector3(0.1f, 0.1f, 0.1f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(-0.1f,  0.1f, 0.1f),
            new Vector3(0.1f, 0.1f, 0.1f), new Vector3( 0.1f, -0.1f, 0.1f), new Vector3(-0.1f, -0.1f, 0.1f)
          },
        new[]
          {
            color, color, color,
            color, color, color
          },
        new []
          {
            new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f),
            new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)
          }
        ); ;
      }
    }
}
