using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Shaders.Parameters;
using OpenTK.Graphics.OpenGL4;

namespace MyDailyLife.Shaders
{
    public class TextureShader : Shader
    {

        private Texture[] Textures { get; set; }

        [Obsolete("use string shader source constructor, this will be removed in next git push")]
        public TextureShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
            Textures = [];
        }

        [Obsolete("use string shader source constructor, this will be removed in next git push")]

        public TextureShader(string vertexPath, string fragmentPath, Texture[] textures) : base(vertexPath, fragmentPath)
        {
            Textures = textures;
        }

        public TextureShader(StringSourceShader source, Texture[] textures) : base(source)
        {
            Textures = textures;
        }

    }
}
