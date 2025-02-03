using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace MyDailyLife.Shaders
{
    public class TextureShader : Shader
    {
        public TextureUnit[] TextureAttributes16 =
        [
            TextureUnit.Texture0,
            TextureUnit.Texture1, 
            TextureUnit.Texture2, 
            TextureUnit.Texture3,
            TextureUnit.Texture4,
            TextureUnit.Texture5,
            TextureUnit.Texture6,
            TextureUnit.Texture7,
            TextureUnit.Texture8,
            TextureUnit.Texture9,
            TextureUnit.Texture10,
            TextureUnit.Texture11,
            TextureUnit.Texture12,
            TextureUnit.Texture13,
            TextureUnit.Texture14,
            TextureUnit.Texture15,
        ];

        public Texture[] Textures { get; private set; }
        public TextureShader(string vertexPath, string fragmentPath, Texture[] textures) : base(vertexPath, fragmentPath)
        {
            if (textures.Length > 16) throw new OverflowException("max texture unit now is only 16");
            Textures = textures;
        }

        private void Activate()
        {
            for(int i = 0; i < Textures.Length; i++)
            {
                Textures[i].Use(TextureAttributes16[i]);
            }
        }

        protected override void ActivateTextures()
        {
            this.Activate();
        }
    }
}
