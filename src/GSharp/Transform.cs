using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;

namespace GSharp
{
  public struct Transform
  {
    public Vector3 Location;
    public Vector3 EulerRotation;
    public Vector4 QuatRotation;
    public Vector3 Scale;
  }
}
