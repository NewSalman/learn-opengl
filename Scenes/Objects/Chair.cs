using MyDailyLife.Meshes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects
{
    public class Box : Drawable
    {
        protected override VertexBuffer CreateBuffer()
        {
            List<Vector3> vertices = new();

            /// create the vertices
            float size = 1f;
            float z = size * 0.2f;

            vertices.AddRange([
                // front
                new(-size, -size, z),
                new(size, -size, z),
                new(size, size, z),
                new(size, size, z),
                new(-size, size, z),
                new(-size, -size, z),

                // size
                new(-size, -size, -z),
                new(size, -size, -z),
                new(size, size, -z),

                new(size, size, -z),
                new(-size, size, -z),
                new(-size, -size, -z),

                // left
                new(-size, -size, z),
                new(-size, size, z),
                new(-size, -size, -z),

                new(-size, -size, -z),
                new(-size, size, -z),
                new(-size, size, z),

                // right
                new(size, -size, z),
                new(size, size, z),
                new(size, -size, -z),
                new(size, -size, -z),
                new(size, size, -z),
                new(size, size, z),

                // top

                new(-size, size, z),
                new(size, size, z),
                new(-size, size, -z),
                new(-size, size, -z),
                new(size, size, -z),
                new(size, size, z),


                // bottom

                new(-size, -size, z),
                new(size, -size, z),
                new(-size, -size, -z),
                new(-size, -size, -z),
                new(size, -size, -z),
                new(size, -size, z),
                ]);

            return new VertexBuffer(Vec3ToFloatArr(vertices), [
                0, 1, 2,
                2, 3, 1
                ], [BufferType.Vertex]);
        }

        protected override Mesh CreateMesh(VertexBuffer buffer)
        {
            return new Mesh(buffer);
        }

        protected override Shader CreateShader()
        {
            return new BasicColorShader("basic/basic.vert", "basic/basic.frag");
        }

        protected override void Draw(double deltatime)
        {

            SetMatrix("model", Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        }
    }
}
