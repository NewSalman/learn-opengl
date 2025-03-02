using MyDailyLife.Meshes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using MyDailyLife.Scenes.Objects.ObjectStructure;
using MyDailyLife.Extension;
using MyDailyLife.Scenes.Objects.Chair.Structures;
using MyDailyLife.Constants;

namespace MyDailyLife.Scenes.Objects.Chair
{
    public class Chair : Drawable<TexturedStructure>
    {
        public static float Width = 1.0f;

        public static float ChairSeatHeight = Width * 0.25f;

        public static float PoleWidth = (Chair.Width * Chair.ChairSeatHeight) * 0.8f;

        public Chair(Matrix4 model) : base(model)
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
            RequestProgramFocus();
            SetInt("material.diffuse", 0);
            SetVector3("material.specular", new(0.04f));
            SetFloat("material.shininess", 50);
        }

        protected override List<TexturedStructure> AddObjectStructures()
        {
            List<TexturedStructure> structures = new();


            Seat seat = new(Model);

            structures.Add(seat);

            float x = -(Width - ChairSeatHeight);
            float y = -(Width);
            float z = Width - ChairSeatHeight;
            float offsetX = x;
            float offsetY = y;
            float offsetZ = z;
            for (int i = 0; i < 4; i++)
            {
                Pole legs = new(Model);

                if (i != 0 && i % 2 == 0)
                {
                    offsetX = x;
                    offsetZ -= z * 2;
                }

                legs.CalculateModel(Matrix4.CreateTranslation(new(offsetX, offsetY, offsetZ)));

                structures.Add(legs);
                offsetX -= x * 2;
            }


            float rX = -Width + 0.25f;
            float rY = Width + ChairSeatHeight * 2;
            float rZ = Width - ChairSeatHeight;

            for (int i = 0; i <= 3; i++)
            {
                BackRestPole backRest = new(Model);


                if (i == 3)
                {

                    TopBackRest topRest = new(Model);

                    topRest.CalculateModel(Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)));
                    topRest.CalculateModel(Matrix4.CreateTranslation(-Width + 0.25f , rY + backRest.Height + topRest.Width, 0));
                    topRest.CalculateModel(Matrix4.CreateRotationY(MathHelper.DegreesToRadians(90f)));

                    structures.Add(topRest);

                    break;
                }

                backRest.CalculateModel(Matrix4.CreateTranslation(new(rX, rY, rZ)));

                structures.Add(backRest);

                rX += 0.75f;
            }

            return structures;
        }

        protected override Mesh CreateMesh(VertexBuffer buffer)
        {
            return new Mesh(buffer);
        }

        protected override Shader CreateShader()
        {
            Texture wood = new Texture("Wood/wood_table_diff_4k.jpg");
            TextureShader shader = new TextureShader("basic/basic.vert", "basic/basic.frag", [wood]);

            shader.ActivateTextures();

            return shader;
        }

        protected override void Draw(double deltatime)
        {
            for(int i = 0; i < DrawableObjects.Count; i++)
            {
                Structure objs = DrawableObjects[i];

                SetMatrix("model", objs.Model);
                SetMatrix("normalMat3", objs.NormalMat);
                GL.DrawArrays(PrimitiveType.Triangles, i * 36, 36);


            }

        }

    }
}
