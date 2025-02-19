using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.UniformBuffers.Types
{
    class LightBufferInfo
    {
        public Vector3 Position { get; set; }
        public nint PosOffset = 0;
        public Vector3 ViewPosition { get; set; }
        public nint ViewPosOffset = 4 * sizeof(float);
        public Vector3 Ambient { get; set; }
        public nint AmbientOffset = 8 * sizeof(float);
        public Vector3 Diffuse { get; set; }
        public nint DiffuseOffset = 12 * sizeof(float);
        public Vector3 Specular { get; set; }
        public nint SpecularOffset = 16 * sizeof(float);

        // 5 vector 3   // Matrix 4 size
        public int Size = 5 * 4 * sizeof(float);

        public int EachSize = 4 * sizeof(float);

        public LightBufferInfo(
            Vector3 position, 
            Vector3 viewPosition, 
            Vector3 ambient, 
            Vector3 diffuse, 
            Vector3 specular)
        {
            Position = position;
            ViewPosition = viewPosition;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }
    }
}
