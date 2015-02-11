using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;

namespace GSharp.Graphics.SDX
{
  public static class ShaderHelper
  {
    public static PixelShader CompileAndBuildPixelShader(Device device, IShaderLoader loader)
    {
      using (var byteCode = loader.LoadByteCode())
      {
        return new PixelShader(device, byteCode);
      }
    }

    public static VertexShader CompileAndBuildVertexShader(Device device, IShaderLoader loader, out ShaderSignature signature)
    {
      using (var byteCode = loader.LoadByteCode())
      {
        signature = ShaderSignature.GetInputSignature(byteCode);
        return new VertexShader(device, byteCode);
      }
    }
  }
}
