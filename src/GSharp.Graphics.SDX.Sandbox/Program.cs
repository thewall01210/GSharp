using System;
using StructureMap;
using StructureMap.Graph;

namespace GSharp.Graphics.SDX.Sandbox
{
  static class Program
  {
    static void Main()
    {
      ObjectFactory.Initialize(x => x.Scan(scan =>
      {
        scan.AssembliesFromPath(Environment.CurrentDirectory);
        scan.LookForRegistries();
      }));

      ObjectFactory.GetInstance<IMainProgram>().Run();
    }
  }
}