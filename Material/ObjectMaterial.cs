using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Material
{
    class ObjectMaterial
    {
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float Shininess { get; set; }

        public ObjectMaterial(Vector3 ambient, Vector3 diffuse, Vector3 specular, float shininess)
        {
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }
    }
}
