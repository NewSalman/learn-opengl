using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Scenes.Objects.ObjectStructure;
using OpenTK.Mathematics;

struct SphereHolder(List<Vector3> vertices, List<Vector3> normals, List<Vector3> textures)
{
    public List<Vector3> Vertices { get; set; } = vertices;
    public List<Vector3> Normals { get; set; } = normals;
    //public List<Vector2> UVs { get; set; } = textures;
    public List<Vector3> Colors { get; set; } = textures;
}

namespace MyDailyLife.Scenes.Objects.Sphere
{
   

    public class SphereStructure : ColoredStrucure
    {
        public SphereStructure(Matrix4 model) : base(model)
        {
        }

        protected override List<Vector3> AddNormals()
        {
            return GenerateAllBuffer().Normals;
        }

        //protected override List<Vector2> AddTextureCoordinate()
        //{
        //    return GenerateAllBuffer().UVs;
        //}

        private SphereHolder GenerateAllBuffer()
        {
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            //List<Vector2> textures = new();
            List<Vector3> colors = new();

            float x, y, z, xy;
            float nx, ny, nz, lengthInv = 1.0f / Sphere.Radius;
            //float u, v;

            float sectorStep = 2 * MathHelper.Pi / Sphere.SectorCount;
            float stackStep = MathHelper.Pi / Sphere.SectorCount;
            float sectorAngle, stackAngle;

            for (int i = 0; i <= Sphere.StackCount; i++)
            {
                stackAngle = MathHelper.Pi / 2 - i * stackStep;
                xy = Sphere.Radius * (float)MathHelper.Cos(stackAngle);
                z = Sphere.Radius * (float)MathHelper.Sin(stackAngle);

                float phi = (float)Math.PI * i / Sphere.StackCount;

                for (int j = 0; j <= Sphere.SectorCount; j++)
                {
                    float theta = 2 * (float)Math.PI * i / Sphere.SectorCount;
                    sectorAngle = j * sectorStep;


                    // just flip it
                    x = xy * (float)MathHelper.Cos(sectorAngle);

                    y = xy * (float)MathHelper.Sin(sectorAngle);

                    vertices.Add(new(x, z, y));

                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;

                    normals.Add(new(nx, ny, nz));

                    //u = theta / (2 * (float)Math.PI);
                    //v = phi / (float)Math.PI;

                    //textures.Add(new(u,v));

                    colors.Add(new(0.53f, 0.81f, 0.92f));


                }
            }


            //return new SphereHolder(vertices, normals, textures);
            return new SphereHolder(vertices, normals, colors);
        }

        protected override List<uint> AddIndices()
        {
            List<uint> indices = new();

            uint k1, k2;
            for (uint i = 0; i < Sphere.StackCount; ++i)
            {
                k1 = i * (uint)(Sphere.SectorCount + 1);
                k2 = k1 + (uint)Sphere.SectorCount + 1;

                for (int j = 0; j < Sphere.SectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }

                    if (i != (Sphere.StackCount - 1))
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }


            return indices;
        }

        protected override List<Vector3> AddVertices()
        {
            return GenerateAllBuffer().Vertices;
        }

        protected override List<Vector3> AddColors()
        {
            // optimized it later
            return GenerateAllBuffer().Colors;
        }
    }
}
