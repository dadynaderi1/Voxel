using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace Voxel;

internal class Shader : IDisposable
{
    public readonly int Handle;

    public Shader(string vertexPath, string fragmentPath)
    {
        // Compile shaders
        int vertexShader = CompileShader(ShaderType.VertexShader, LoadShaderSource(vertexPath));
        int fragmentShader = CompileShader(ShaderType.FragmentShader, LoadShaderSource(fragmentPath));

        // Create program
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);
        GL.LinkProgram(Handle);

        // Check for linking errors
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int linked);
        if (linked == 0)
        {
            string infoLog = GL.GetProgramInfoLog(Handle);
            throw new Exception($"Error linking shader program: {infoLog}");
        }

        // Cleanup shaders
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Handle);
    }

    private static int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int compiled);
        if (compiled == 0)
        {
            string infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error compiling {type} shader: {infoLog}");
        }

        return shader;
    }

    private static string LoadShaderSource(string filePath)
    {
        try
        {
            string fullPath = Path.Combine("../../../shaders", filePath);
            return File.ReadAllText(fullPath);
        }
        catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
        {
            throw new Exception($"Failed to load shader file: {e.Message}");
        }
    }
}
