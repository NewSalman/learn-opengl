using MyDailyLife.Extension;
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

    public class ColorMeshVec3(Vector3 vertex, Vector3 normal, Vector3 color)
    {
        public Vector3 Vertex = vertex;
        public Vector3 Normal = normal;
        public Vector3 Color = color;
    }

    public struct UVMeshVec3
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 UV;
    } 

    abstract public class Mesh : IDisposable
    {
        public Color4? Color { get; set; }
        public Shader Shader { get; set; }
        protected Vertex[] Vertecies {  get; set; }
        protected uint[] Indices { get; set; }
        //protected Vector3[] Positions { get; set; }
        protected int Vao { get; set; }
        protected int Vbo { get; set; }
        protected int Ebo { get; set; }

        protected float[] Buffer { get; set; }

        public Matrix4[] Models { get; set; }


        [Obsolete("use vec3 as input constructor")]
        public Mesh(Vertex[] vertecies, uint[] indices, Shader shader, Matrix4[] models)
        {
            Indices = indices;

            Vertecies = vertecies;

            Shader = shader;

            Models = models;
            
            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);

            Initialize();

            Shader.Use();

            ActivateUBO();

            GL.BindVertexArray(0);
        }

        public Mesh(ColorMeshVec3[] data, uint[] indices, Shader shader)
        {
            Indices = indices;
            Buffer = MergeVec3F(data);
            Shader = shader;

            CreateVAO();

            CreateVBO();

            CreateEBO();

            EnableVertexAttrib();

            Shader.Use();

            ActivateUBO();
        }

        private void CreateVAO()
        {
            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);
        }

        private void CreateVBO()
        {
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Buffer.Length * sizeof(float), Buffer, BufferUsageHint.StaticDraw);
        }

        private void CreateEBO()
        {
            Ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
        }

        private void EnableVertexAttrib()
        {
            int stride = 9 * sizeof(float);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }

        private void ActivateUBO()
        {
            int uniformBlockIndex = Shader.GetUniformBlockIndex("Matrices");

            Shader.SetUniformBlockBinding(uniformBlockIndex, Constants.CameraUniformBufferPoint);
        }

        private float[] MergeVec3F(ColorMeshVec3[] data)
        {
            float[] bufferData = [];

            for (int i = 0; i < data.Length; i++)
            {
                bufferData = [.. bufferData, .. data[i].Vertex.ToArray(), .. data[i].Normal.ToArray(), .. data[i].Color.ToArray()];
            }

            return bufferData;
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
