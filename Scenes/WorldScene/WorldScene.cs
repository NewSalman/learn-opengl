using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Constants;
using MyDailyLife.Material;
using MyDailyLife.Objects.Shape.Cylinder;
using MyDailyLife.Scenes.Components;
using MyDailyLife.UniformBuffers;
using MyDailyLife.UniformBuffers.Types;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyDailyLife.Scenes.WorldScene
{
    class WorldScene : Scene
    {
        private UniformBuffer LightUniform;

        private Light Sun;
        private Vector3 LightPosition;
        private float Angle = 0.0f;
        private float Radius = 3.5f;

        public WorldScene(float aspecRatio) : base(aspecRatio)
        {
            LightPosition = CalculateLighOrbit();

            //LightMaterial ligthMat = new LightMaterial(
            //    position: LightPosition,
            //    viewPosition: new(),
            //    ambient: new(0.2f, 0.2f, 0.2f),
            //    diffuse: new(0.5f, 0.5f, 0.5f),
            //    specular: new(1.0f, 1.0f, 1.0f)
            //);

            Sun = new Light();
            Sun.LightPos = Vector3.UnitZ * 2;

            AddObject([new Cylinder(1.0f, 3.0f, 64, withCover: true)]);
            LightUniform = new(5 * 3 * sizeof(float), UBO.LightPositionBlockPoint);

            LightUniform.BindDataVector(new UniformBufferData<Vector3>(3 * sizeof(float), 0, new Vector3(0.3f, 0.552f, 0.7f)));
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

            LightPosition = new Vector3(lightX, LightPosition.Y, lightZ);

            return LightPosition;
        }

        public override void Render(double deltaTime)
        {

            LightPosition = CalculateLighOrbit();
            //Sun.LightPos = LightPosition;

            LightUniform.BindDataVector(new UniformBufferData<Vector3>(3 * sizeof(float), 0, LightPosition));


            base.Render(deltaTime);
        }

        protected override void Release()
        {
            base.Release();

            LightUniform.Dispose();
        }
    }
}
