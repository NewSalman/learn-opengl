using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Scenes.Objects.ObjectStructure;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.Ground
{
    public class GroundStructure : TexturedStructure
    {
        public GroundStructure(Matrix4 model) : base(model)
        {
        }

        protected override List<OpenTK.Mathematics.Vector3> AddNormals()
        {
            return [
                new(0.0f, 0.0f, 1.0f),
                new(0.0f, 0.0f, 1.0f),
                new(0.0f, 0.0f, 1.0f),

                new(0.0f, 0.0f, 1.0f),
                new(0.0f, 0.0f, 1.0f),
                new(0.0f, 0.0f, 1.0f),


                new(0.0f, 0.0f, -1.0f),
                new(0.0f, 0.0f, -1.0f),
                new(0.0f, 0.0f, -1.0f),

                new(0.0f, 0.0f, -1.0f),
                new(0.0f, 0.0f, -1.0f),
                new(0.0f, 0.0f, -1.0f),


                new(-1.0f, 0.0f, 0.0f),
                new(-1.0f, 0.0f, 0.0f),
                new(-1.0f, 0.0f, 0.0f),

                new(-1.0f, 0.0f, 0.0f),
                new(-1.0f, 0.0f, 0.0f),
                new(-1.0f, 0.0f, 0.0f),


                new(1.0f, 0.0f, 0.0f),
                new(1.0f, 0.0f, 0.0f),
                new(1.0f, 0.0f, 0.0f),

                new(1.0f, 0.0f, 0.0f),
                new(1.0f, 0.0f, 0.0f),
                new(1.0f, 0.0f, 0.0f),


                new(0.0f, 1.0f, 0.0f),
                new(0.0f, 1.0f, 0.0f),
                new(0.0f, 1.0f, 0.0f),

                new(0.0f, 1.0f, 0.0f),
                new(0.0f, 1.0f, 0.0f),
                new(0.0f, 1.0f, 0.0f),


                new(0.0f, -1.0f, 0.0f),
                new(0.0f, -1.0f, 0.0f),
                new(0.0f, -1.0f, 0.0f),

                new(0.0f, -1.0f, 0.0f),
                new(0.0f, -1.0f, 0.0f),
                new(0.0f, -1.0f, 0.0f),
            ];
        }

        protected override List<Vector2> AddTextureCoordinate()
        {
            return [
                new(0.0f, 0.0f),
                new(1.0f, 0.0f),
                new(1.0f, 1.0f),

                new(1.0f, 1.0f),
                new(0.0f, 1.0f),
                new(0.0f, 0.0f),

                new(0.0f, 0.0f),
                new(1.0f, 0.0f),
                new(1.0f, 1.0f),

                new(1.0f, 1.0f),
                new(0.0f, 1.0f),
                new(0.0f, 0.0f),

                new(1.0f, 0.0f),
                new(1.0f, 1.0f),
                new(0.0f, 0.0f),

                new(0.0f, 0.0f),
                new(0.0f, 1.0f),
                new(1.0f, 1.0f),

                new(0.0f, 0.0f),
                new(0.0f, 1.0f),
                new(1.0f, 0.0f),

                new(1.0f, 0.0f),
                new(1.0f, 1.0f),
                new(0.0f, 1.0f),

                new(0.0f, 0.0f),
                new(1.0f, 0.0f),
                new(0.0f, 1.0f),

                new(0.0f, 1.0f),
                new(1.0f, 1.0f),
                new(1.0f, 0.0f),

                new(0.0f, 0.0f),
                new(1.0f, 0.0f),
                new(0.0f, 1.0f),

                new(0.0f, 1.0f),
                new(1.0f, 1.0f),
                new(1.0f, 0.0f),
            ];
        }

        protected override List<OpenTK.Mathematics.Vector3> AddVertices()
        {
            List<Vector3> vertices = new();

            /// create the vertices
            float size = Ground.Width;
            float z = Ground.Height;

            vertices.AddRange([
                // front
                new(-size, -z, size),
                new(size, -z, size),
                new(size, z, size),

                new(size, z, size),
                new(-size, z, size),
                new(-size, -z, size),

                // back
                new(-size, -z, -size),
                new(size, -z, -size),
                new(size, z, -size),

                new(size, z, -size),
                new(-size, z, -size),
                new(-size, -z, -size),

                // left
                new(-size, -z, size),
                new(-size, z, size),
                new(-size, -z, -size),

                new(-size, -z, -size),
                new(-size, z, -size),
                new(-size, z, size),

                // right
                new(size, -z, size),
                new(size, z, size),
                new(size, -z, -size),

                new(size, -z, -size),
                new(size, z, -size),
                new(size, z, size),

                // top

                new(-size, z, size),
                new(size, z, size),
                new(-size, z, -size),

                new(-size, z, -size),
                new(size, z, -size),
                new(size, z, size),


                // bottom

                new(-size, -z, size),
                new(size, -z, size),
                new(-size, -z, -size),

                new(-size, -z, -size),
                new(size, -z, -size),
                new(size, -z, size),
            ]);

            return vertices;
        }
    }
}
