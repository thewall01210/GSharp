using SlimDX.D3DCompiler;

namespace GSharp.Graphics.SDX
{
  public interface IShaderLoader
  {
    ShaderBytecode LoadByteCode();
  }
}