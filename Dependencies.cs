using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDailyLife
{
    public sealed class Dependencies
    {
        private static Lazy<TextureLoader> _textureLoader = new(() => new TextureLoader());
        public static TextureLoader TextureLoader { get { return _textureLoader.Value; } }
    }
}
