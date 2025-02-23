using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Constants;
using MyDailyLife.Extension;
using MyDailyLife.Scenes.Objects;
using MyDailyLife.UniformBuffers;
using MyDailyLife.UniformBuffers.Types;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyDailyLife.Scenes.WorldScene
{
    class WorldScene : Scene
    {
        //private UniformBuffer _lightUniform;

        //private UniformBufferData<Vector3> _lightPosition;
        //private UniformBufferData<Vector3> _viewPosition;
        //private UniformBufferData<Vector3> _lightAmbient;
        //private UniformBufferData<Vector3> _lightDiffuse;
        //private UniformBufferData<Vector3> _lightSpecular;

        private Chair _box;
        
        private float Angle = 0.0f;
        private float Radius = 3.2f;

        public WorldScene(float aspecRatio) : base(aspecRatio)
        {
            Vector3 lightPos = CalculateLighOrbit();
            _box = new();
        }

        private Vector3 CalculateLighOrbit()
        {
            Angle += 4.0f * (float)DeltaTime * Speed;

            if (Angle > 360.0f)
            {
                Angle -= 360.0f;
            }

            float orbitAndle = MathHelper.DegreesToRadians(Angle);

            float lightX = Radius * (float)MathHelper.Cos(orbitAndle);
            float lightZ = Radius * (float)MathHelper.Sin(orbitAndle);

            return new Vector3(lightX, -0.5f, lightZ);
            //return new Vector3(lightX, lightZ, 1.0f);
        }

        public override void Render(double deltaTime)
        {

            base.Render(deltaTime);

            _box.Render(deltaTime);
        }

        protected override void Release()
        {
            _box.Dispose();
            base.Release();
        }
    }
}
