using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Meshes;
using MyDailyLife.Scenes.Objects.ObjectStructure;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.Sphere
{
    public class Sphere : Drawable<ColoredStrucure>
    {
        public static int SectorCount = 32;
        public static int StackCount = 32;
        public static float Radius = 4f;
        public Sphere(Matrix4 model) : base(model)
        {
        }

        protected override void Initialized()
        {
            base.Initialized();

            //RequestProgramFocus();
            //SetInt("material.diffuse", 0);

        }

        protected override List<ColoredStrucure> AddObjectStructures()
        {
            return [new SphereStructure(Model)];
        }

        protected override Mesh CreateMesh(VertexBuffer buffer)
        {
            return new Mesh(buffer);
        }

        protected override Shader CreateShader()
        {
            //Texture wood = new Texture("skies/qwantani_puresky.jpg");
            //Texture wood = new Texture("skies/qwantani_puresky.jpg");
            BasicColorShader shader = new("Sky/Sphere.vert", "Sky/Sphere.frag");

            //shader.ActivateTextures();

            return shader;
        }

        protected override void Draw(double deltatime)
        {
            SetMatrix("model", Model);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawElements(PrimitiveType.Triangles, Buffer!.Indices.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
