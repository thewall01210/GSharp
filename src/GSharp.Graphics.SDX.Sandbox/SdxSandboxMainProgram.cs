using System.Windows.Forms;
using System.Diagnostics;
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
    private Device device;
    private SwapChain swapChain;
    private RenderForm form;
    private SwapChainDescription description;
    private RenderTargetView renderTarget;
    private DeviceContext context;
    private Viewport viewport;

    private Stopwatch gameTimer = new Stopwatch();
    private Stopwatch graphicsTimer = new Stopwatch();
    private float elapsedSec = 0.0f;
    private int frames = 0;
    private float fpsSecCounter = 0.0f;

    public void Run()
    {
      form = new RenderForm("Sandbox");

      description = new SwapChainDescription()
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

      // create a view of our render target, which is the back-buffer of the swap chain we just created
      using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
      {
        renderTarget = new RenderTargetView(device, resource);
      }

      // setting a viewport is required if you want to actually see anything
      context = device.ImmediateContext;
      viewport = new Viewport(0.0f, 0.0f, form.ClientSize.Width, form.ClientSize.Height);
      context.OutputMerger.SetTargets(renderTarget);
      context.Rasterizer.SetViewports(viewport);

      // load and compile the vertex shader
      ShaderSignature inputSignature;

      var vertexShaderLoader =
        new ShaderFileLoader(
          "Shaders/DirectionalShader.fx",
          "VShader",
          "vs_4_0",
          ShaderFlags.None,
          EffectFlags.None);

      var pixelShaderLoader =
        new ShaderFileLoader(
          "Shaders/DirectionalShader.fx",
          "PShader",
          "ps_4_0",
          ShaderFlags.None,
          EffectFlags.None);

      var vertexShader =
        ShaderHelper.CompileAndBuildVertexShader(device, vertexShaderLoader, out inputSignature);
      var pixelShader =
        ShaderHelper.CompileAndBuildPixelShader(device, pixelShaderLoader);

      var blueBox = ContentLoader.GetColoredBox(new Vector3(0.3f, 0.5f, 0.7f));

      var inputLayout = new InputLayout(device, inputSignature, blueBox.GetSharderInputElements());

      var vertexBuffer = new Buffer(
          device,
          blueBox.DataStream,
          blueBox.BufferSize,
          ResourceUsage.Default,
          BindFlags.VertexBuffer,
          CpuAccessFlags.None,
          ResourceOptionFlags.None,
          0);
      
      var vertexBufferBindings = new VertexBufferBinding[]
      {
        new VertexBufferBinding(vertexBuffer, ColoredModel.VertexSize, 0)
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

      KeyBindings();

      MessagePump.Run(form, () =>
      {
        // Loop start
        elapsedSec = GetTotalElapsedSeconds();
        gameTimer.Restart();
        FrameCounter();

        // Game Update
        gameTimer.Restart();
        UpdateGameLogic();
        gameTimer.Stop();

        // Loop end with graphics update
        context.ClearRenderTargetView(renderTarget, new Color4(1.0f, 0.1f, 0.1f, 0.1f));
        context.Draw(blueBox.BufferSize, 0);
        swapChain.Present(0, PresentFlags.None);
      });

      // clean up all resources
      // anything we missed will show up in the debug output
      blueBox.Dispose();
      vertexBuffer.Dispose();
      inputLayout.Dispose();
      inputSignature.Dispose();
      vertexShader.Dispose();
      pixelShader.Dispose();
      renderTarget.Dispose();
      swapChain.Dispose();
      device.Dispose();
    }

    private void KeyBindings()
    { // 
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
    }

    private void UpdateGameLogic()
    {


      for (var i = 0; i < 2000000; i++ )
      {
        var speedUpLoop = 1.0 / 3.0 / 5.0 / 7.0;
        speedUpLoop /= speedUpLoop;
      }
    }

    private void FrameCounter()
    {
      fpsSecCounter += elapsedSec;
      if (fpsSecCounter > 1.0f)
      {
        fpsSecCounter -= 1.0f;
        System.Diagnostics.Debug.WriteLine(frames);
        frames = 0;
      }
      frames++;
    }

    private float GetTotalElapsedSeconds()
    {
      return 0.001f * gameTimer.ElapsedMilliseconds;
    }
  }
}
