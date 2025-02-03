using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Objects
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SharedMatrix
    {
        Matrix4 View;
        Matrix4 Projection;
    }
}
