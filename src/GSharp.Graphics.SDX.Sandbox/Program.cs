using SlimDX.Windows;

namespace GSharp.Graphics.SDX.Sandbox
{
  static class Program
  {
    static void Main()
    {
      // http://slimdx.org/tutorials/BasicWindow.php
      var form = new RenderForm("Sandbox");
      MessagePump.Run(form, () => { });
    }
  }
}