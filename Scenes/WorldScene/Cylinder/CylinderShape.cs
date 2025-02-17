using MyDailyLife.Constants;
using MyDailyLife.Material;
using MyDailyLife.Meshes;
using MyDailyLife.Scenes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Objects.Shape.Cylinder
{
    public class CylinderShape : Drawable, IDisposable
    {
        public float TopRadius {  get; set; }
        public float BottomRadius {  get; set; }
        private int Slices { get; set; }
        private float Height { get; set; }

        private bool WithCover = true;

        private bool TopBase = true;
        private bool BottomBase = true;

        public CylinderShape(float radius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool withCover = true) : base()
        {
            TopRadius = radius;
            BottomRadius = radius;

            Slices = slices;
            Height = height;

            //CylinderMesh = mesh;
            //CylinderShader = shader;

            Models = models ?? [Matrix4.Identity];

            WithCover = withCover;

            Initialize();
        }

        public CylinderShape(float topRadius, float bottomRadius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool withCover = true) : base()
        {
            TopRadius = topRadius;
            BottomRadius = bottomRadius;

            Slices = slices;
            Height = height;

            //CylinderMesh = mesh;
            //CylinderShader = shader;

            Models = models ?? [Matrix4.Identity];

            WithCover = withCover;

            //ActivateUBO(UBO.ClippedSpaceBlockKey, UBO.ClippedSpaceBlockIndex);
            Initialize();
        }

        public CylinderShape(float topRadius, float bottomRadius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool topBase = true, bool bottomBase = true) : base()
        {
            TopRadius = topRadius;
            BottomRadius = bottomRadius;

            Slices = slices;
            Height = height;

            //CylinderMesh = mesh;
            //CylinderShader = shader;

            Models = models ?? [Matrix4.Identity];

            WithCover = true;
            TopBase = topBase;
            BottomBase = bottomBase;

            //ActivateUBO(UBO.ClippedSpaceBlockKey, UBO.ClippedSpaceBlockIndex);
            Initialize();
        }

        protected override void InitializeUBOBinding(Shader shader)
        {
            base.InitializeUBOBinding(shader);

            int lightBlockIndex = shader.GetUniformBlockIndex(UBO.LightPositionBlockKey);
            shader.SetUniformBlockBinding(lightBlockIndex, UBO.LightPositionBlockPoint);
        }

        protected override void AfterObjectCreated()
        {
            //SetMatrix("");
        }
        protected override Matrix4[] CreateModels()
        {
            return [Matrix4.Identity];
        }

        protected override Shader CreateShader()
        {
            return new BasicColorShader("Scenes/MainScene/cylinder/vert.glsl", "Scenes/MainScene/cylinder/frag.glsl");
        }

        protected override void OnDraw(double DeltaTime)
        {
            for (int i = 0; i < Models.Length; i++)
            {

                SetMatrix("model", Models[i]);
                // Drawing the top circle

                // Render cylinder side first
                // 1 Slice contain 2 triangle
                // first = number of slices

                GL.DrawElements(PrimitiveType.Triangles, Geometry.Indices.Length, DrawElementsType.UnsignedInt, 0);


                if (WithCover)
                {
                    if (TopBase)
                    {
                        // top
                        GL.DrawArrays(PrimitiveType.TriangleFan, Slices * 2 + 3, Slices + 1);
                    }


                    if (BottomBase)
                    {
                        // bottom
                        GL.DrawArrays(PrimitiveType.TriangleFan, Slices * 3 + 5, Slices + 1);
                    }
                }



                // Render top cover
                // first = after the current lenght of side = number vertecies slices

                // Render bottom cover
                // first = after the current lenght of side plus the number of top bottom vertices = number vertecies slices + top bottom vertices
                //GL.DrawArrays(PrimitiveType.Triangles, NumVerticesSide + NumVerticesTopBottom, NumVerticesTopBottom);
            }
        }

        protected override Mesh CreateMesh(Geometry geometry)
        {
            return new Mesh(geometry);
        }

        protected override Geometry CreateGeometry()
        {
            List<float> vertecies = [];
            List<uint> indices = [];

            List<float> x = new();
            List<float> y = new();

            float height = 3.0f / 2.0f;
            //Slices = 32;
            //TopRadius = 2.0f;
            //BottomRadius = 3.5f;


            for (int i = 0; i <= Slices; i++)
            {
                float angle = 2.0f * MathHelper.Pi * i / Slices;

                x.Add((float)MathHelper.Cos(angle));
                y.Add((float)MathHelper.Sin(angle));
            }

            for (int i = 0; i <= Slices; i++)
            {
                vertecies.AddRange([TopRadius * x[i], height, TopRadius * y[i], x[i], y[i], 0.0f]);
            }

            for (int i = 0; i <= Slices; i++)
            {
                vertecies.AddRange([BottomRadius * x[i], -height, BottomRadius * y[i], x[i], y[i], 0.0f]);
            }


            vertecies.AddRange([TopRadius * 0.0f, height, TopRadius * 0.0f, 0.0f, 1.0f, 0.0f]);
            for (int i = 0; i <= Slices; i++)
            {
                vertecies.AddRange([TopRadius * x[i], height, TopRadius * y[i], 0.0f, 1.0f, 0.0f]);
            }


            vertecies.AddRange([BottomRadius * 0.0f, -height, BottomRadius * 0.0f, 0.0f, -1.0f, 0.0f]);
            for (int i = 0; i <= Slices; i++)
            {
                vertecies.AddRange([BottomRadius * x[i], -height, BottomRadius * y[i], 0.0f, -1.0f, 0.0f]);
            }

            //int k1 = 0;                         // 1st vertex index at base
            //int k2 = sectorCount + 1;           // 1st vertex index at top
            // create side vertices indices
            // 1st vertex index at base or top lah


            /// loop for each segment/slice
            /// each slice have 6 indices
            /// and if finish go to next slice (i++)
            uint k1 = 0;
            uint k2 = (uint)Slices + 1;
            for (uint i = 1; i <= Slices; i++, k1++, k2++)
            {
                uint v1 = k1;
                uint v2 = k1 + 1;
                uint v3 = k2;

                uint v4 = k2;
                uint v5 = k1 + 1;
                uint v6 = k2 + 1;



                indices.AddRange([v1, v2, v3, v4, v5, v6]);
            }

            return new Geometry()
            {
                Buffer = vertecies.ToArray(),
                Indices = indices.ToArray(),
                BufferBinding = BufferBinding.Vertices_Normals
            };
        }
    }
}
