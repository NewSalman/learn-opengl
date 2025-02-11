using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Extension
{
    public static class Vector2Ext
    {
        public static float[] ToArray(this Vector2 vector) => [vector.X, vector.Y];
    }
}
