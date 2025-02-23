using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife.Scenes.Objects.Structure
{
    public abstract class TexturedStructure : Structure
    {
        public List<Vector2> TextureCoordinate { get; private set; }

        public TexturedStructure()
        {
            TextureCoordinate = AddTextureCoordinate();
        }
        abstract protected List<Vector2> AddTextureCoordinate();
    }
}
