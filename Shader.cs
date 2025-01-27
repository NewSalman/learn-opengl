using OpenTK.Compute.OpenCL;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Resources;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyDailyLife
{
    public class Shader
    {
        public int Handle;

        private readonly int VertexShader;
        private readonly int FragmentShader;
        private bool disposedValue = false;

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource = File.ReadAllText($"Shaders/{vertexPath}");
            string fragmentShaderSource = File.ReadAllText($"Shaders/{fragmentPath}");

            VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, vertexShaderSource);

            FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, fragmentShaderSource);

            GL.CompileShader(VertexShader);

            {
                GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                {
                    Console.WriteLine("Failed compile vertex shader with error: {0}", GL.GetShaderInfoLog(VertexShader));
                }
            }

            {
                GL.CompileShader(FragmentShader);

                GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success);
                if (success == 0)
                {
                    Console.WriteLine("Failed compile fragment shader with error: {0}", GL.GetShaderInfoLog(FragmentShader));
                }
            }

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            {
                GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
                if (success == 0)
                {
                    Console.WriteLine("Failed to link program with error: {0}", GL.GetProgramInfoLog(Handle));
                }
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(VertexShader);
            GL.DeleteShader(FragmentShader);
        }


        public void Use()
        {
            GL.UseProgram(Handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }

        public void SetInt(string name, int value)
        {
            int uniformLocation = GL.GetUniformLocation(Handle, name);

            GL.Uniform1(uniformLocation, value);
        }

        public void SetMatrix4(string name, ref Matrix4 value)
        {
            int location = GL.GetUniformLocation(Handle, name);
            GL.UniformMatrix4(location, true, ref value);
        }

        ~Shader()
        {
            if(!disposedValue)
            {
                Console.WriteLine("GPU Resource leak, Resource not disposed correctly");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
