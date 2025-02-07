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
        public Cube(Vertex[] vertecies, uint[] indices, Shader shader, Matrix4[] models) : base(vertecies, indices, shader, models)
        {
        }

        protected override void Draw(double time)
        {

            for (int i = 0; i < Models.Length; i++)
            {
                // or i don't need that before draw, i just can modify on loop, but it's makes that main loop messy

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            }
        }

        protected override void Initialize()
        {
            float[] vboData = MergeArrays();
            int stride = Vertecies[0].Size * sizeof(float);

            //Vbo = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            //GL.BufferData(BufferTarget.ArrayBuffer, vboData.Length * sizeof(float), vboData, BufferUsageHint.StaticDraw);

            //Ebo = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            // position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            //normals
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            for (int i = 0; i < Models.Length; i++)
            {
                Shader.SetMatrix4("model", Models[i]);
            }

            // texture coordinate
            //GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
            //GL.EnableVertexAttribArray(2);


            /// TODO : ====================== handle this on the shader ==========================
            //Shader.SetInt("texture0", 0);
            //Shader.SetInt("texture1", 1);

        }
    }
}
