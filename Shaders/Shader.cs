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
using MyDailyLife.Objects;

namespace MyDailyLife.Shaders
{
    abstract public class Shader
    {
        public int Handle;

        private readonly int VertexShader;
        private readonly int FragmentShader;
        private bool disposedValue = false;
        //private int SharedUniformBlockIndex;

        private Dictionary<string, int> _uniformLocations = [];

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource = File.ReadAllText($"Assets/Shaders/{vertexPath}");
            string fragmentShaderSource = File.ReadAllText($"Assets/Shaders/{fragmentPath}");

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


            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numOfUniforms);

            for (int i = 0; i < numOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);

                int location = GL.GetUniformLocation(Handle, key);

                _uniformLocations.Add(key, location);
            }
        }


        public void Use()
        {
            GL.UseProgram(Handle);

            this.ActivateTextures();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);
                disposedValue = true;
            }
        }
        abstract protected void ActivateTextures();

        public void SetInt(string name, int value)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], value);
        }

        public void SetMatrix4(string name, Matrix4 value)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref value);
        }

        public void SetMatrix4(Dictionary<string, Matrix4> uniforms)
        {
            if (uniforms.Count <= 0) return;

            GL.UseProgram(Handle);
            foreach(KeyValuePair<string, Matrix4> uniform in uniforms)
            {
                Matrix4 value = uniform.Value;

                GL.UniformMatrix4(_uniformLocations[uniform.Key], true , ref value);
            }
        }

        public void SetVec3(string name, Vector3 value)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], value);
        }

        public void SetVec3(Dictionary<string, Vector3> uniforms)
        {
            if (uniforms.Count <= 0) return;

            GL.UseProgram(Handle);

            foreach (KeyValuePair<string, Vector3> uniform in uniforms)
            {
                GL.Uniform3(_uniformLocations[uniform.Key], uniform.Value);
            }
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(Handle, name);
        }

        public void SetUniformBlockBinding(int blockIndex, int blockPoint)
        {
            GL.UniformBlockBinding(Handle, blockIndex, blockPoint);
        }

        public int GetUniformBlockIndex(string name)
        {
            return GL.GetUniformBlockIndex(Handle, name);
        }

        ~Shader()
        {
            if (!disposedValue)
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
