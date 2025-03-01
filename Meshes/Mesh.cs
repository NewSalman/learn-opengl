using System;
using System.Drawing;
using System.Xml.Linq;
using MyDailyLife.Extension;
using MyDailyLife.Scenes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Meshes
{
    public class Mesh
    {
        private int _vao;
        private int _vbo;
        private int _ebo;
        protected float[] Buffer { get; set; }
        protected uint[] Indices { get; set; } = [];

        protected List<BufferType> BufferOrder;


        public Mesh(VertexBuffer buffer)
        {
            Buffer = buffer.Data.ToArray();
            Indices = buffer.Indices.ToArray();
            BufferOrder = buffer.BufferOrder;

            CreateVAO();
            OnVertexArrayBinded(() =>
            {
                CreateVBO();

                CreateEBO();

                EnableVertexAttribs();
            });
        }

        protected void CreateVAO()
        {
            _vao = GL.GenVertexArray();
        }

        protected void CreateVBO()
        {
            float[] testData = new float[Buffer.Length];
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Buffer.Length * sizeof(float), Buffer, BufferUsageHint.StaticDraw);
        }

        protected void CreateEBO()
        {
            if (Indices.Length > 0)
            {
                _ebo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
            }
        }

        protected void EnableVertexAttribs()
        {
            int stride = CountStride();
            int nextBufferOffset = 0;
            for (int i = 0; i < BufferOrder.Count; i++)
            {
                BufferType currentOrder = BufferOrder[i];

                switch (currentOrder)
                {
                    case BufferType.Vertex:
                    case BufferType.Normal:
                    case BufferType.Color:
                        GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, stride, nextBufferOffset);
                        GL.EnableVertexAttribArray(i);
                        nextBufferOffset += 3 * sizeof(float);
                        break;
                    case BufferType.TextureCoordinate:
                        GL.VertexAttribPointer(i, 2, VertexAttribPointerType.Float, false, stride, nextBufferOffset);
                        GL.EnableVertexAttribArray(i);
                        nextBufferOffset += 2 * sizeof(float);
                        break;
                }

            }
        }

        private int CountStride()
        {
            int stride = 0;
            for (int i = 0; i < BufferOrder.Count; i++)
            {
                BufferType currentOrder = BufferOrder[i];

                switch (currentOrder)
                {
                    case BufferType.Vertex:
                    case BufferType.Normal:
                    case BufferType.Color:
                        stride += 3 * sizeof(float);
                        break;
                    case BufferType.TextureCoordinate:
                        stride += 2 * sizeof(float);
                        break;
                }

            }

            return stride;
        }
        
        public void OnVertexArrayBinded(Action action)
        {
            GL.BindVertexArray(_vao);
            action();
            GL.BindVertexArray(0);
        } 

        public void Dispose()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vbo);
            GL.DeleteBuffer(_ebo);
            GL.DeleteVertexArray(_vao);
        }

    }
}
