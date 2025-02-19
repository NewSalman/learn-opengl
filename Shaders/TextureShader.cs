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
        public TextureUnit[] TextureAttributes =
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
            TextureUnit.Texture16,
            TextureUnit.Texture17,
            TextureUnit.Texture18,
            TextureUnit.Texture19,
            TextureUnit.Texture20,
            TextureUnit.Texture21,
            TextureUnit.Texture22,
            TextureUnit.Texture23,
            TextureUnit.Texture24,
            TextureUnit.Texture25,
            TextureUnit.Texture26,
            TextureUnit.Texture27,
            TextureUnit.Texture28,
            TextureUnit.Texture29,
            TextureUnit.Texture30,
            TextureUnit.Texture31
        ];

        private int MaxTextureUnit { get; set; }
        public Texture[] Textures { get; set; }
        public TextureShader(string vertexPath, string fragmentPath) : base(vertexPath, fragmentPath)
        {
            CheckMaxTextureUnit();

            Textures = [];
        }

        public TextureShader(string vertexPath, string fragmentPath, Texture[] textures) : base(vertexPath, fragmentPath)
        {
            Textures = textures;
        }

        private void CheckMaxTextureUnit()
        {
            MaxTextureUnit = GL.GetInteger(GetPName.MaxTextureUnits);
        }
        public override void ActivateTextures()
        {
            for (int i = 0; i < Textures.Length; i++)
            {
                Textures[i].Use(TextureAttributes[i]);
            }
        }
    }
}
