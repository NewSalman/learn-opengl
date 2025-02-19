using MyDailyLife.Constants;
using MyDailyLife.Material;
using MyDailyLife.Meshes;
using MyDailyLife.Scenes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Objects.Shape.Cylinder
{
    public class Cylinder : Drawable, IDisposable
    {
        public float TopRadius {  get; set; }
        public float BottomRadius {  get; set; }
        private int Slices { get; set; }
        private float Height { get; set; }

        private bool WithCover = true;

        private bool TopBase = true;
        private bool BottomBase = true;

        public Cylinder(float radius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool withCover = true) : base()
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

        public Cylinder(float topRadius, float bottomRadius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool withCover = true) : base()
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

        public Cylinder(float topRadius, float bottomRadius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool topBase = true, bool bottomBase = true) : base()
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
            Dictionary<string, Vector3> materialVec = new();

            //materialVec.Add("material.ambient", new(0.24725f, 0.1995f, 0.0745f));
            //materialVec.Add("material.diffuse", new(0.75164f, 0.60648f, 0.22648f));

            //materialVec.Add("material.diffuse", new Vector3(0.75164f, 0.60648f, 0.22648f));
            materialVec.Add("material.specular", new(0.628281f, 0.555802f, 0.366065f));
            UseProgram((shader) =>
            {
                //SetInt("material.diffuse", 0);
                shader.ActivateTextures();

                SetFloat("material.shininess", 0.4f);
                
                SetVectors3(materialVec);
            });

        }

        protected override Matrix4[] CreateModels()
        {
            return [Matrix4.Identity];
        }

        protected override Shader CreateShader()
        {
            Texture concreteTexture = new Texture("Concrete/concrete_diff.jpg");
            TextureShader shader = new TextureShader("Shapes/Cylinder/vert.glsl", "Shapes/Cylinder/frag.glsl", [concreteTexture]);
            return shader;
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
            List<Vector2> textureCoordinate = [];

            List<float> x = new();
            List<float> y = new();

            float height = Height / 2.0f;


            for (int i = 0; i <= Slices; i++)
            {
                float angle = 2.0f * MathHelper.Pi * i / Slices;

                float vertX = (float)MathHelper.Cos(angle);
                float vertY = (float)MathHelper.Sin(angle);
                x.Add(vertX);
                y.Add(vertY);

                Vector2 uv = new();

                uv.X = 0.5f + ((float)MathHelper.Cos(angle) * 0.5f);
                uv.Y = 0.5f + ((float)MathHelper.Sin(angle) * 0.5f);

                textureCoordinate.Add(uv);
            }

            for (int i = 0; i <= Slices; i++)
            {
                vertecies.AddRange([TopRadius * x[i], height, TopRadius * y[i], x[i], y[i], 0.0f, (float)i / Slices, 1.0f]);
            }

            for (int i = 0; i <= Slices; i++)
            {
                vertecies.AddRange([BottomRadius * x[i], -height, BottomRadius * y[i], x[i], y[i], 0.0f, (float)i / Slices, 0.0f]);
            }


            vertecies.AddRange([0.0f, height,0.0f, 0.0f, 1.0f, 0.0f, 0.5f, 0.5f]);
            for (int i = 0; i <= Slices; i++)
            {
                Vector2 uv = textureCoordinate[i];
                vertecies.AddRange([TopRadius * x[i], height, TopRadius * y[i], 0.0f, 1.0f, 0.0f, uv.X, uv.Y]);
            }


            vertecies.AddRange([0.0f, -height, 0.0f, 0.0f, -1.0f, 0.0f, 0.5f, 0.5f]);
            for (int i = 0; i <= Slices; i++)
            {
                Vector2 uv = textureCoordinate[i];
                vertecies.AddRange([BottomRadius * x[i], -height, BottomRadius * y[i], 0.0f, -1.0f, 0.0f, uv.X, uv.Y]);
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
                BufferBinding = BufferBinding.Vertices_Normals_Texture_Coordinates
            };
        }
    }
}
