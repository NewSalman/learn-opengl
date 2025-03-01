using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.Chair.Structures
{
    public class BackRestPole : Pole
    {
        public BackRestPole(Matrix4 model) : base(model)
        {
        }

        public override float Height { get; set; } = Chair.Width * 1.25f;
        public override float Width { get; set; } = Chair.Width * 0.15f;
    }
}
