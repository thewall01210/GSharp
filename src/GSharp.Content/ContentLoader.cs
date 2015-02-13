using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using SlimDX.Direct3D11;

namespace GSharp.Content
{
    public static class ContentLoader
    {
      public static Texture2D GetTexture(Device device, string fileName)
      {
        return Texture2D.FromFile(device, fileName);
      }
    }
}
