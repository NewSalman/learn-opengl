using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife.Scenes.Objects.Structure
{
    public abstract class Structure
    {
        public List<Vector3> Vertices { get; private set; }
        public List<Vector3> Normals { get; private set; }

        public Structure()
        {
            Vertices = AddVertices();
            Normals = AddNormals();
        }
        abstract public List<Vector3> AddVertices();
        abstract public List<Vector3> AddNormals();

    }
}
