using StructureMap.Configuration.DSL;

namespace GSharp.Graphics.SDX.Sandbox
{
  public sealed class SdxSandboxRegistry : Registry
  {
    public SdxSandboxRegistry()
    {
      For<IMainProgram>().Use<SdxSandboxMainProgram>();
    }
  }
}