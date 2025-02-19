using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Constants;
using MyDailyLife.Extension;
using MyDailyLife.Material;
using MyDailyLife.Objects.Shape.Cylinder;
using MyDailyLife.Scenes.Components;
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
        private UniformBuffer _lightUniform;
        private LightBufferInfo _lightBufferInfo;

        private Light Sun;
        private float Angle = 0.0f;
        private float Radius = 3.2f;

        private UniformBufferData<Vector3>[] _lightUniformBuffer = new UniformBufferData<Vector3>[5];
        //private float[] _lightUniformBuffer = new float[5*3];

        public WorldScene(float aspecRatio) : base(aspecRatio)
        {
            Vector3 lightPos = CalculateLighOrbit();
            _lightBufferInfo = new(
                position: lightPos,
                viewPosition: MainCamera.Position,
                ambient: new(0.844f),
                diffuse: new(0.8655f),
                specular: new(0.6f)
            );

            _lightUniform = new(_lightBufferInfo.Size, UBO.LightPositionBlockPoint);

            _lightUniformBuffer[0] = new UniformBufferData<Vector3>(_lightBufferInfo.EachSize, _lightBufferInfo.PosOffset, _lightBufferInfo.Position);
            _lightUniformBuffer[1] = new UniformBufferData<Vector3>(_lightBufferInfo.EachSize, _lightBufferInfo.ViewPosOffset, _lightBufferInfo.ViewPosition);
            _lightUniformBuffer[2] = new UniformBufferData<Vector3>(_lightBufferInfo.EachSize, _lightBufferInfo.AmbientOffset, _lightBufferInfo.Ambient);
            _lightUniformBuffer[3] = new UniformBufferData<Vector3>(_lightBufferInfo.EachSize, _lightBufferInfo.DiffuseOffset, _lightBufferInfo.Diffuse);
            _lightUniformBuffer[4] = new UniformBufferData<Vector3>(_lightBufferInfo.EachSize, _lightBufferInfo.SpecularOffset, _lightBufferInfo.Specular);

            _lightUniform.BindDataVectors(_lightUniformBuffer);

            Sun = new Light();
            Sun.LightPos = lightPos;

            AddObject([Sun, new Cylinder(2.0f, 2.0f, 160, height: 3, withCover: true)]);

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

            return new Vector3(lightX, 1.0f, lightZ);
            //return new Vector3(lightX, lightZ, 1.0f);
        }

        public override void Render(double deltaTime)
        {
            Vector3 lightPosition = CalculateLighOrbit();

            _lightBufferInfo.Position = lightPosition;
            _lightBufferInfo.ViewPosition = MainCamera.Position;

            Sun.LightPos = lightPosition;

            /// why just assign new class of UniformBufferdata? 
            /// bcuz i want to try as low memory usage as posible, so just update the existing data
            /// since i know where the position of the position and the view position

            // update position buffer data
            _lightUniformBuffer[0].Data = _lightBufferInfo.Position;

            // update view position buffer data
            _lightUniformBuffer[1].Data = _lightBufferInfo.ViewPosition;

            //Vector3 lightColor = new((float)MathHelper.Sin((float)deltaTime * 0.4f), (float)MathHelper.Sin((float)deltaTime * 1.7f), (float)MathHelper.Sin((float)deltaTime * 2.1f));
            //Vector3 diffuse = lightColor * new Vector3(0.5f);
            //Vector3 ambient = diffuse * new Vector3(0.2f);

            //_lightUniformBuffer[2].Data = ambient;
            //_lightUniformBuffer[3].Data = diffuse;


            _lightUniform.BindDataVectors([
                _lightUniformBuffer[0],
                _lightUniformBuffer[1],
                //_lightUniformBuffer[2],
                //_lightUniformBuffer[3],
            ]);

            base.Render(deltaTime);
        }

        protected override void Release()
        {
            base.Release();

            _lightUniform.Dispose();
        }
    }
}
