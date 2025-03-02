using MyDailyLife.Constants;
using MyDailyLife.Meshes;
using MyDailyLife.Scenes.Objects.ObjectStructure;
using MyDailyLife.Shaders;
using MyDailyLife.Shaders.Parameters;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.Ground
{
    public class Ground : Drawable<TexturedStructure>
    {
        public static float Height = Width * 0.25f;
        public static float Width = 1.0f;

        public Ground(Matrix4 model) : base(model)
        {
        }

        protected override void BindUBO(Shader shader)
        {
            base.BindUBO(shader);

            int lightBlockIndex = shader.GetUniformBlockIndex(UBO.LightPositionBlockKey);
            shader.SetUniformBlockBinding(lightBlockIndex, UBO.LightPositionBlockPoint);
        }

        protected override void Initialized()
        {
            base.Initialized();
            RequestProgramFocus();

            SetInt("material.diffuse", 0);
            SetInt("material.normal", 1);
            //SetInt("material.arm", 2);
        }

        protected override List<TexturedStructure> AddObjectStructures()
        {
            return [new GroundStructure(Model)];
        }

        protected override Mesh CreateMesh(VertexBuffer buffer)
        {
            return new Mesh(buffer);
        }

        protected override Shader CreateShader()
        {
            string path = "Ground/Granite/4K/";
            Texture diffuse = new Texture(path + "granite_tile_diff.jpg");
            Texture normal = new Texture(path + "granite_tile_nor.jpg");
            //Texture arm = new Texture(path + "granite_tile_arm.jpg");

            TextureShader shader = new(new StringSourceShader(GroundShaderSource.VertexSource, GroundShaderSource.FragmentSource), [diffuse, normal]);
            shader.ActivateTextures();
            return shader;
        }

        protected override void Draw(double deltatime)
        {
            SetMatrix("model", Matrix4.CreateTranslation(new(0, 1, 0)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}
