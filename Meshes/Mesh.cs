using System.Drawing;
using System.Xml.Linq;
using MyDailyLife.Extension;
using MyDailyLife.Scenes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Meshes
{
    public enum BufferBinding
    {
        Vertices_Only,
        Vertices_Normals,
        Vertices_Colors,
        Vertices_Normals_Colors,
        Vertices_Normals_Texture_Coordinates,
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

        public Mesh(Geometry geometry)
        {
            Buffer = geometry.Buffer;
            Indices = geometry.Indices;

            CreateVAO();

            CreateVBO();

            CreateEBO();

            EnableVertexAttrib(geometry.BufferBinding);
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

        protected override void EnableVertexAttrib(BufferBinding binding = BufferBinding.Vertices_Normals_Colors)
        {
            int stride = 9 * sizeof(float);

            switch (binding)
            {
                case BufferBinding.Vertices_Normals_Colors:
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
                    GL.EnableVertexAttribArray(1);

                    GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
                    GL.EnableVertexAttribArray(2);
                    break;

                case BufferBinding.Vertices_Normals_Texture_Coordinates:
                    stride = 8 * sizeof(float);

                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
                    GL.EnableVertexAttribArray(1);

                    GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
                    GL.EnableVertexAttribArray(2);
                    break;

                case BufferBinding.Vertices_Normals:
                    stride = 6 * sizeof(float);

                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
                    GL.EnableVertexAttribArray(0);

                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
                    GL.EnableVertexAttribArray(1);
                    break;

                case BufferBinding.Vertices_Colors:
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

        public override void Render(double deltaTime) 
        {
            GL.BindVertexArray(Vao);
            OnDraw(deltaTime);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            Shader?.Dispose();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(Vbo);
            GL.DeleteBuffer(Ebo);
            GL.DeleteVertexArray(Vao);
        }

    }
}
