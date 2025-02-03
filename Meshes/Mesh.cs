using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Objects;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Meshes
{
    enum BufferMergeState
    {
        OnVerteciesLoop,
        OnNormalsLoop,
        OnTextCoordinatesLoop,
    }
    abstract public class Mesh : IDisposable
    {
        public Color4? Color { get; set; }
        public Shader Shader { get; set; }
        protected Vertex[] Vertecies {  get; set; }
        protected uint[] Indices { get; set; }
        protected Vector3[] Positions { get; set; }
        protected int Vao { get; set; }
        protected int Vbo { get; set; }
        protected int Ebo { get; set; }

        public Mesh(Vertex[] vertecies, uint[] indices, Shader shader, Vector3[] position)
        {
            Indices = indices;

            Vertecies = vertecies;

            Shader = shader;

            Positions = position;

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);

            Shader.Use();

            Initialize();

            int uniformBlockIndex = Shader.GetUniformBlockIndex("Matrices");

            Shader.SetUniformBlockBinding(uniformBlockIndex, Constants.CameraUniformBufferPoint);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            GL.BindVertexArray(0);
        }

        abstract protected void Initialize();

        protected float[] MergeArrays()
        {
            int bufferLength = Vertecies[0].Size * Vertecies.Length;

            float[] result = new float[bufferLength];

            for (int i = 0; i < Vertecies.Length; i++) 
            {
                Vertecies[i].MergeVertex().CopyTo(result, Vertecies[i].Size * i);
            }

            return result;
        }

        abstract protected void Draw(double time);

        public void Render(double time) 
        {
            GL.BindVertexArray(Vao);
            Shader.Use();
            Draw(time);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            Shader.Dispose();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(Vbo);
            GL.DeleteBuffer(Ebo);
            GL.DeleteVertexArray(Vao);
        }
    }
}
