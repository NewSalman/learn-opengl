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
        private float LightSize = 0.2f;
        private Vector3 LightPosition;
        private float Angle = 0.0f;
        private float Radius = 3.5f;
        private float Speed = 2.0f;


        protected override UniformBuffer CameraUniform { get; set; }
        protected override Camera MainCamera { get; set; }



        public WorldScene(float aspecRatio)
        {
            LightPosition = CalculateLighOrbit();

            LightMaterial ligthMat = new LightMaterial(
                position: LightPosition,
                viewPosition: new(),
                ambient: new(0.2f, 0.2f, 0.2f),
                diffuse: new(0.5f, 0.5f, 0.5f),
                specular: new(1.0f, 1.0f, 1.0f)
            );

            Sun = new Light();
            AddObject([Sun, new Cylinder(1.0f, 3.0f, 64, withCover: true)]);

            MainCamera = new Camera(Vector3.UnitZ * 3, aspecRatio);

            LightUniform = new(3 * 4 * sizeof(float), UBO.LightPositionBlockPoint);
            CameraUniform = new(CameraBufferInfo.Size, UBO.CameraBlockPoint);

            //LightUniform.BindDataVector(new UniformBufferData<Vector3>(3 * sizeof(float), 0, new Vector3(0.3f, 0.552f, 0.7f)));
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
            Sun.LightPos = LightPosition;

            //LightUniform.BindDataVector(new UniformBufferData<Vector3>(3 * sizeof(float), 0, 
            //    new Vector3(0.5f * (float)MathHelper.Sin(deltaTime))));

            LightUniform.BindDataVector(new UniformBufferData<Vector3>(3 * sizeof(float), 0, LightPosition));
            CameraUniform.BindDataMatrices([
                new UniformBufferData<Matrix4>(CameraBufferInfo.EachSize, CameraBufferInfo.ViewOffset, MainCamera.GetViewMatrix()),
                new UniformBufferData<Matrix4>(CameraBufferInfo.EachSize, CameraBufferInfo.ProjectionOffset, MainCamera.GetProjectionMatrix())
            ]);

            base.Render(deltaTime);
        }

        protected override void Release()
        {
            base.Release();

            LightUniform.Dispose();
        }
    }
}
