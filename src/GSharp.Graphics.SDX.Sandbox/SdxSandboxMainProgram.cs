using System.Windows.Forms;
using GSharp.Content;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Resource = SlimDX.Direct3D11.Resource;

namespace GSharp.Graphics.SDX.Sandbox
{
  public sealed class SdxSandboxMainProgram : IMainProgram
  {
    public void Run()
    {
      Device device;
      SwapChain swapChain;

      var form = new RenderForm("Sandbox");
      var description = new SwapChainDescription()
      {
        BufferCount = 2,
        Usage = Usage.RenderTargetOutput,
        OutputHandle = form.Handle,
        IsWindowed = true,
        ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
        SampleDescription = new SampleDescription(1, 0),
        Flags = SwapChainFlags.AllowModeSwitch,
        SwapEffect = SwapEffect.Discard
      };

      Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out device, out swapChain);

      // var squareTexture = ContentLoader.GetTexture(device, "box.bmp");

      // create a view of our render target, which is the back-buffer of the swap chain we just created
      RenderTargetView renderTarget;
      using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
      {
        renderTarget = new RenderTargetView(device, resource);
      }

      // setting a viewport is required if you want to actually see anything
      var context = device.ImmediateContext;
      var viewport = new Viewport(0.0f, 0.0f, form.ClientSize.Width, form.ClientSize.Height);
      context.OutputMerger.SetTargets(renderTarget);
      context.Rasterizer.SetViewports(viewport);

      // load and compile the vertex shader
      ShaderSignature inputSignature;

      var vertexShaderLoader =
        new ShaderFileLoader(
          "Shaders/PassThrough.fx",
          "VShader",
          "vs_4_0",
          ShaderFlags.None,
          EffectFlags.None);

      var pixelShaderLoader =
        new ShaderFileLoader(
          "Shaders/PassThrough.fx",
          "PShader",
          "ps_4_0",
          ShaderFlags.None,
          EffectFlags.None);

      var vertexShader =
        ShaderHelper.CompileAndBuildVertexShader(device, vertexShaderLoader, out inputSignature);
      var pixelShader =
        ShaderHelper.CompileAndBuildPixelShader(device, pixelShaderLoader);

      var model = new Model(
        new[]
          {
            new Vector3(0.1f, 0.1f, 0.1f), new Vector3(-0.1f, -0.1f, 0.1f), new Vector3(-0.1f, 0.1f, 0.1f),
            new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.1f, -0.1f, 0.1f), new Vector3(-0.1f, -0.1f, 0.1f)
          },
          new[]
          {
            new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f)
          }
        );

      // create the vertex layout and buffer
      var elements = new []
      {
        new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
        new InputElement("COLOR", 0, Format.R32G32B32_Float, Model.Vector3Size * model.Vertices.Length, 0)
      };

      var inputLayout = new InputLayout(device, inputSignature, elements);

      var vertexBuffer = new Buffer(
          device,
          model.DataStream,
          model.BufferSize,
          ResourceUsage.Default,
          BindFlags.VertexBuffer,
          CpuAccessFlags.None,
          ResourceOptionFlags.None,
          0);
      

      var vertexBufferBindings = new VertexBufferBinding[]
      {
        new VertexBufferBinding(vertexBuffer, Model.Vector3Size, 0)
      };

      // configure the Input Assembler portion of the pipeline with the vertex data
      context.InputAssembler.InputLayout = inputLayout;
      context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
      context.InputAssembler.SetVertexBuffers(0, vertexBufferBindings);


      // set the shaders
      context.VertexShader.Set(vertexShader);
      context.PixelShader.Set(pixelShader);

      // prevent DXGI handling of alt+enter, which doesn't work properly with Winforms
      // full screen below in "form.KeyDown"
      using (var factory = swapChain.GetParent<Factory>())
      {
        factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);
      }

      form.KeyDown += (o, e) =>
      {
        // full screen
        if (e.Alt && e.KeyCode == Keys.Enter)
          swapChain.IsFullScreen = !swapChain.IsFullScreen;

        // closes the graphics window
        if (e.KeyCode == Keys.Escape)
        {
          form.Close();
        }
      };

      // handle form size changes
      form.UserResized += (o, e) =>
      {
        renderTarget.Dispose();

        swapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
        using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
        {
          renderTarget = new RenderTargetView(device, resource);
        }

        context.OutputMerger.SetTargets(renderTarget);
      };

      MessagePump.Run(form, () =>
      {
        // clear the render target to grey
        context.ClearRenderTargetView(renderTarget, new Color4(1.0f, 0.1f, 0.1f, 0.1f));

        // draw the triangle
        context.Draw(model.BufferSize, 0);
        swapChain.Present(0, PresentFlags.None);
      });

      // clean up all resources
      // anything we missed will show up in the debug output
      model.Dispose();
      vertexBuffer.Dispose();
      inputLayout.Dispose();
      inputSignature.Dispose();
      vertexShader.Dispose();
      pixelShader.Dispose();
      renderTarget.Dispose();
      swapChain.Dispose();
      device.Dispose();
    }
  }
}