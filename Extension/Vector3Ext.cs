using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Extension
{
    public static class Vector3Ext
    {
        public static float[] ToArray(this Vector3 vector) => [vector.X, vector.Y, vector.Z];

    }
}
