using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Meshes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyDailyLife.Objects
{
    public class Cube : Mesh
    {
        public Cube(Vertex[] vertecies, uint[] indices, Shader shader, Vector3[] position) : base(vertecies, indices, shader, position)
        {
        }

        protected override void Draw(double time)
        {
            float totalDegres = (float)MathHelper.DegreesToRadians(time);

            for (int i = 0; i < Positions.Length; i++)
            {
                Matrix4 model = Matrix4.CreateRotationY(totalDegres);
                Matrix4 translate = Matrix4.CreateTranslation(Positions[i]);

                model *= translate;

                Shader.SetMatrix4("model", model);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            }
        }

        protected override void Initialize()
        {
            float[] vboData = MergeArrays();
            int stride = Vertecies[0].Size * sizeof(float);

            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vboData.Length * sizeof(float), vboData, BufferUsageHint.StaticDraw);

            Ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            // position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            //normals
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texture coordinate
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            Shader.SetInt("texture0", 0);
            Shader.SetInt("texture1", 1);

        }
    }
}
