using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife.Shaders.Parameters
{
    public class FileSourceShader(string vertexPath, string fragmentPath)
    {
        public String VertexSource { get; private set; } = File.ReadAllText(vertexPath);
        public String FragmentSource { get; private set; } = File.ReadAllText(fragmentPath);

    }
}
