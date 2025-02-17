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
    public class CubeEBO
    {
        //public CubeEBO(Vertex[] vertecies, uint[] indices, Shader shader, Matrix4[] models) : base(vertecies, indices, shader, models)
        //{
        //}

        //protected override void Initialize()
        //{
        //    float[] vboData = MergeArrays();
        //    int stride = Vertecies[0].Size * sizeof(float);

        //    Vbo = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
        //    GL.BufferData(BufferTarget.ArrayBuffer, vboData.Length * sizeof(float), vboData, BufferUsageHint.StaticDraw);

        //    Ebo = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

        //    // position
        //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
        //    GL.EnableVertexAttribArray(0);

        //    //normals
        //    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
        //    GL.EnableVertexAttribArray(1);


        //    // Color
        //    GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));
        //    GL.EnableVertexAttribArray(2);

        //}
        
        //protected void Draw(double time)
        //{
        //    for (int i = 0; i < Models.Length; i++) 
        //    {
        //        //Matrix4 model = Matrix4.Identity;
        //        //model *= Matrix4.CreateScale(1f);
        //        //Matrix4 translate = Matrix4.CreateTranslation();

        //        //model *= translate;

        //        Shader.SetMatrix4("model", Models[i]);
        //        GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        //    }
        //}

    }
}
