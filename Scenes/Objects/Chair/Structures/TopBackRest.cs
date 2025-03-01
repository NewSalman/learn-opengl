using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.Chair.Structures
{
    public class TopBackRest : Pole
    {
        public TopBackRest(Matrix4 model) : base(model)
        {
        }

        public override float Height { get; set; } = Chair.Width;
    }
}
