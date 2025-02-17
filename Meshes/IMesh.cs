using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Meshes
{
    abstract public class IMesh
    {
        protected int Vao { get; set; }
        protected int Vbo { get; set; }
        protected int Ebo { get; set; }
        public Action<double> OnDraw { get; set; }
        public Matrix4[] Models { get; set; } = [];
        abstract protected void CreateVAO();

        abstract protected void CreateVBO();

        abstract protected void CreateEBO();

        abstract protected void EnableVertexAttrib(BufferBinding binding);

        abstract public void Render(double deltaTime);
    }
}
