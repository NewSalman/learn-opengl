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
    public class Cylinder : IDisposable
    {
        public float TopRadius {  get; set; }
        public float BottomRadius {  get; set; }
        private int Slices { get; set; }
        private float Height { get; set; }
        private Circle TopBase {  get; set; }
        private Circle BottomBase {  get; set; }
        private Shader? CylinderShader { get; set; }
        private Mesh? CylinderMesh { get; set; }
        private List<ColorVertex> Buffer { get; set; } = [];
        private Matrix4[] Models = [];

        private readonly bool WithCover;

        public Cylinder(float radius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool withCover = true)
        {
            TopRadius = radius;
            BottomRadius = radius;

            Slices = slices;
            Height = height;

            CylinderMesh = mesh;
            CylinderShader = shader;

            Models = models ?? [Matrix4.Identity];

            TopBase = new Circle(Slices, radius);
            BottomBase = new Circle(Slices, radius);

            WithCover = withCover;

            CalculateBaseCircle();

            InitializeDrawArraysIndexer();
        }

        public Cylinder(float topRadius, float bottomRadius, int slices, float height = 3.0f, Mesh? mesh = null, Shader? shader = null, Matrix4[]? models = null, bool withCover = true)
        {
            TopRadius = topRadius;
            BottomRadius = bottomRadius;

            Slices = slices;
            Height = height;

            CylinderMesh = mesh;
            CylinderShader = shader;

            Models = models ?? [Matrix4.Identity];

            TopBase = new Circle(Slices, TopRadius);
            BottomBase = new Circle(Slices, BottomRadius);

            WithCover = withCover;

            CalculateBaseCircle();

            InitializeDrawArraysIndexer();
        }

        private void InitializeShaders(float[] buffer, uint[] indices)
        {
            int topSlicesCount = Slices + 2;
            int topSlicesIndex = 0;

            int bottomSlicesIndex = Slices + 2;
            int bottomSlicesCount = Slices + 2;

            int sideSlicesIndex = topSlicesCount * 2;
            int sideSlicesCount = Slices * 2;

            CylinderShader = CylinderShader ?? new BasicColorShader("basic/Circle/circle.vert", "basic/Circle/circle.frag");

            CylinderMesh = CylinderMesh ?? new Mesh(buffer, indices, BufferBinding.Vertices_Normal);

            CylinderMesh.Shader = CylinderShader;
            CylinderMesh.ActivateUBO(Constants.ClippedSpaceBlockKey, Constants.ClippedSpaceBlockIndex);

            CylinderMesh!.OnDraw = (deltaTime) => {
                for (int i = 0; i < Models.Length; i++)
                {

                    CylinderShader.SetMatrix4("model", Models[i]);
                    // Drawing the top circle

                    // Render cylinder side first
                    // 1 Slice contain 2 triangle
                    // first = number of slices

                    //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


                    if (WithCover)
                    {
                        // top
                        GL.DrawArrays(PrimitiveType.TriangleFan, Slices * 2 + 3, Slices + 1);


                        // bottom
                        GL.DrawArrays(PrimitiveType.TriangleFan, Slices * 3 + 5, Slices + 1);
                    }



                    // Render top cover
                    // first = after the current lenght of side = number vertecies slices

                    // Render bottom cover
                    // first = after the current lenght of side plus the number of top bottom vertices = number vertecies slices + top bottom vertices
                    //GL.DrawArrays(PrimitiveType.Triangles, NumVerticesSide + NumVerticesTopBottom, NumVerticesTopBottom);
                }
            };


        }

        private void InitializeDrawArraysIndexer()
        {
            
        }

        public void Render(double deltaTime)
        {
            CylinderMesh.Render(deltaTime);
        }


        private void CalculateBaseCircle()
        {
            List<float> vertecies = [];
            List<uint> indices = [];

            List<float> x = new();
            List<float> y = new();

            float height = Height / 2.0f;


            for (int i = 0; i <= Slices; i++)
            {
                float angle = 2.0f * MathHelper.Pi * i / Slices;

                x.Add((float)MathHelper.Cos(angle));
                y.Add((float)MathHelper.Sin(angle));
            }

            for(int i = 0; i <= Slices; i++)
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



            InitializeShaders([.. vertecies], [.. indices]);
        }

        public void Dispose()
        {
            CylinderMesh.Dispose();
        }
    }
}
