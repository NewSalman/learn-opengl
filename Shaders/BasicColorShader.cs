using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife.Shaders
{
    public class BasicColorShader : Shader
    {
        public BasicColorShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
        }

        public override void ActivateTextures()
        {
            
        }
    }
}
