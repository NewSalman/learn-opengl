using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.UniformBuffers.Types
{
    public class CameraBufferInfo
    {
        public static int EachSize = 4 * 4 * sizeof(float);
        public static int Size = 2 * (4 * 4 * sizeof(float));

        public Matrix4 View { get; set; }
        public static nint ViewOffset = 0;

        public Matrix4 Projection { get; set; }
        public static nint ProjectionOffset = 4 * 4 * sizeof(float);

        public CameraBufferInfo(Matrix4 view, Matrix4 projection)
        {
            View = view;
            Projection = projection;
        }
    }
}
