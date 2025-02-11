using System.Drawing;
using System.Xml.Linq;
using MyDailyLife.Extension;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Meshes
{
    public enum BufferBinding
    {
        Vertices_Only,
        Vertices_Normal,
        Vertices_Normal_Color,
        Vertices_Normal_Texture_Coordinate,
    }

    public class Vertex(Vector3 vertex, Vector3 normal)
    {
        public Vector3 Position = vertex;
        public Vector3 Normal = normal;
    }

    public class ColorVertex(Vector3 vertex, Vector3 normal, Vector3 color) : Vertex(vertex, normal)
    {
        public Vector3 Color { get; private set; } = color;
    }

    public class TextureVertex(Vector3 vertex, Vector3 normal, Vector2 uv) : Vertex(vertex, normal)
    {
        public Vector2 TextureCoordinate { get; private set; } = uv;
    }

    public class Mesh : IMesh, IDisposable
    {
        public Color4? Color { get; set; }
        public Shader Shader { get; set; }
        protected Vertex[] Vertecies {  get; set; }
        public uint[] Indices { get; set; } = [];

        protected float[] Buffer { get; set; }

        public Mesh(Vertex[] data, uint[] indices, Shader shader)
        {
            Buffer = MergeVec3F(data);
            Shader = shader;

            CreateVAO();

            CreateVBO();

            CreateEBO();

            EnableVertexAttrib();

            Shader.Use();
        }

        public Mesh(Vertex[] data, uint[] indices)
        {
            Buffer = MergeVec3F(data);
            Indices = indices;

            CreateVAO();

            CreateVBO();

            CreateEBO();

            if (data[0] is TextureVertex)
            {
                EnableVertexAttrib(BufferBinding.Vertices_Normal_Texture_Coordinate);
            } else
            {
                EnableVertexAttrib();
            }

        }

        public Mesh(float[] data, uint[] indices, BufferBinding bufferBinding)
        {
            Buffer = data;
            Indices = indices;

            CreateVAO();

            CreateVBO();

            CreateEBO();

            EnableVertexAttrib(bufferBinding);

        }

        protected override void CreateVAO()
        {
            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);
        }

        protected override void CreateVBO()
        {
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Buffer.Length * sizeof(float), Buffer, BufferUsageHint.StaticDraw);
        }

        protected override void CreateEBO()
        {
            if (Indices.Length > 0)
            {
                Ebo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
            }
        }

        protected override void EnableVertexAttrib(BufferBinding binding = BufferBinding.Vertices_Normal_Color)
        {
            int stride = 9 * sizeof(float);

            switch (binding)
            {
                case BufferBinding.Vertices_Normal_Color:
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
                    GL.EnableVertexAttribArray(1);

                    GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
                    GL.EnableVertexAttribArray(2);
                    break;

                case BufferBinding.Vertices_Normal_Texture_Coordinate:
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
                    GL.EnableVertexAttribArray(1);

                    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
                    GL.EnableVertexAttribArray(2);
                    break;

                case BufferBinding.Vertices_Normal:
                    stride = 6 * sizeof(float);

                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
                    GL.EnableVertexAttribArray(1);
                    break;

                case BufferBinding.Vertices_Only:
                    stride = 3 * sizeof(float);

                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);
                    break;

            }


            GL.BindVertexArray(0);
        }

        public void ActivateUBO(string name, int index)
        {
            EnableUBOblock(name, index);
        }


        protected override void EnableUBOblock(string name, int index)
        {
            Shader.Use();

            int uniformBlockIndex = Shader.GetUniformBlockIndex(name);

            Shader.SetUniformBlockBinding(uniformBlockIndex, index);
        }

        private float[] MergeVec3F(Vertex[] data)
        {
            float[] bufferData = [];

            for (int i = 0; i < data.Length; i++)
            {
                if (data is ColorVertex[])
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    ColorVertex vertex = data[i] as ColorVertex;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    bufferData = [.. bufferData, .. vertex!.Position.ToArray(), .. vertex.Normal.ToArray(), .. vertex.Color.ToArray()];
                } else
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    TextureVertex vertex = data[i] as TextureVertex;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                    bufferData = [.. bufferData, .. vertex!.Position.ToArray(), .. vertex.Normal.ToArray(), .. vertex.TextureCoordinate.ToArray()];


                }
            }

            return bufferData;
        }

        protected float[] MergeArrays()
        {
            int bufferLength = 0;

            float[] result = new float[bufferLength];

            //for (int i = 0; i < Vertecies.Length; i++) 
            //{
            //    Vertecies[i].MergeVertex().CopyTo(result, Vertecies[i].Size * i);
            //}

            return result;
        }

        public override void Render(double deltaTime) 
        {
            GL.BindVertexArray(Vao);
            OnDraw(deltaTime);
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
