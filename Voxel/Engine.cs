using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Voxel;

internal class Engine : GameWindow
{
    private readonly float[] _vertices =
    {
        0f, 0.5f, 0f, // Top vertex
        -0.5f, -0.5f, 0f, // Bottom left vertex
        0.5f, -0.5f, 0f // Bottom right vertex
    };

    private int _vao;
    private Shader? _shader;
    private readonly int _width;
    private readonly int _height;

    public Engine(int width, int height) 
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        _width = width;
        _height = height;
        CenterWindow(new Vector2i(width, height));
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);

        Console.WriteLine($"Width is {e.Width}");
        Console.WriteLine($"Height is {e.Height}");
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        _vao = BufferManager.CreateVertexArray(_vertices); // Vertex buffer setup moved to BufferManager
        
        _shader = new Shader("Default.vert", "Default.frag");
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        GL.DeleteVertexArray(_vao);
        _shader.Dispose();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.ClearColor(1f, 1f, 1f, 1f);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader.Use();
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        Context.SwapBuffers();
        base.OnRenderFrame(args);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }
}