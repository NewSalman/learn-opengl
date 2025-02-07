using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Meshes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Objects
{
    public class Circle : Mesh
    {
        private int Count {  get; set; } 
        public Circle(ColorMeshVec3[] data, uint[] indices, Shader shader, int count) : base(data, indices, shader)
        {
            Count = count;
        }

        protected override void Draw(double time)
        {
            for (int i = 0; i < Count; i++)
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }
        }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
