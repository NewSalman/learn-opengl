using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife.Shaders.Parameters
{
    public class StringSourceShader(string vertexSource, string fragmentSource)
    {
        public String VertexSource { get; private set; } = vertexSource;
        public String FragmentSource { get; private set; } = fragmentSource;
    }
}
