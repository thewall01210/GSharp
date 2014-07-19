using SlimDX.D3DCompiler;

namespace GSharp.Graphics.SDX
{
  public class ShaderFileLoader : IShaderLoader
  {
    public ShaderFileLoader(string fileName, string entryPoint, string profile, ShaderFlags shaderFlags, EffectFlags effectFlags)
    {
      FileName = fileName;
      EntryPoint = entryPoint;
      Profile = profile;
      ShaderFlags = shaderFlags;
      EffectFlags = effectFlags;
    }

    public string FileName { get; set; }

    public string EntryPoint { get; set; }

    public string Profile { get; set; }

    public ShaderFlags ShaderFlags { get; set; }

    public EffectFlags EffectFlags { get; set; }

    public ShaderBytecode LoadByteCode()
    {
      return ShaderBytecode.CompileFromFile(FileName, EntryPoint, Profile, ShaderFlags, EffectFlags);
    }
  }
}