using MyDailyLife.Constants;
using MyDailyLife.UniformBuffers;
using MyDailyLife.UniformBuffers.Types;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyDailyLife.Scenes
{
    public abstract class Scene : IDisposable
    {
        private bool _firstMove = true;
        private Vector2 _lastPosition;
        private bool _freeMouse = false;


        private float angle = 0.0f;
        private float radius = 3.2f;
        public static Vector3 LightPosition { get; private set; }

        private UniformBuffer CameraUniform { get; set; }
        private UniformBufferData<Matrix4>[] CameraUniformData = new UniformBufferData<Matrix4>[2];


        protected UniformBuffer LightUniform;
        protected UniformBufferData<Vector3>[] LightUniformData = new UniformBufferData<Vector3>[5];

        protected Camera MainCamera { get; set; }

        private double _deltaTime;
        protected double DeltaTime
        {
            get => _deltaTime;
        }

        private float _speed = 2.0f;
        protected float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
            }
        }

        private float _sensitivity = 0.5f;
        protected float Sensitivity
        {
            get => _sensitivity;
            set
            {
                _sensitivity = value;
            }
        }

        public Scene(float aspectRatio)
        {

            MainCamera = new Camera(Vector3.UnitZ * 5, aspectRatio);

            CameraUniform = new(CameraBufferInfo.Size, UBO.CameraBlockPoint);
            CameraUniformData[0] = new UniformBufferData<Matrix4>(CameraBufferInfo.EachSize, CameraBufferInfo.ViewOffset, MainCamera.GetViewMatrix());
            CameraUniformData[1] = new UniformBufferData<Matrix4>(CameraBufferInfo.EachSize, CameraBufferInfo.ProjectionOffset, MainCamera.GetProjectionMatrix());


            CalculateLighOrbit();
            // vec3 become vec4 in shader, it's why 4 times of sizeof(float) not 3
            int uboLightSize = 5 * 4 * sizeof(float);
            int lightVecSize = 4 * sizeof(float);
            LightUniform = new(uboLightSize, UBO.LightPositionBlockPoint);

            // light position
            LightUniformData[0] = new(lightVecSize, 0, LightPosition);

            // view position
            LightUniformData[1] = new(lightVecSize, lightVecSize, MainCamera.Position);

            // light ambient
            LightUniformData[2] = new(lightVecSize, 2 * lightVecSize, new(0.4663f));

            // light diffuse
            LightUniformData[3] = new(lightVecSize, 3 * lightVecSize, new(0.7343f));

            // light specular
            LightUniformData[4] = new(lightVecSize, 4 * lightVecSize, new(1.0f));

            LightUniform.BindDataVectors(LightUniformData);
        }

        private void CalculateLighOrbit()
        {
            angle += 4.0f * (float)DeltaTime * Speed;

            if (angle > 360.0f)
            {
                angle -= 360.0f;
            }

            float orbitAndle = MathHelper.DegreesToRadians(angle);

            float lightX = radius * (float)MathHelper.Cos(orbitAndle);
            float lightZ = radius * (float)MathHelper.Sin(orbitAndle);

            LightPosition =  new Vector3(lightX, -0.5f, lightZ);
            //return new Vector3(lightX, lightZ, 1.0f);
        }

        public virtual void Render(double deltaTime)
        {
            _deltaTime = deltaTime;

            CalculateLighOrbit();

            CameraUniformData[0].Data = MainCamera.GetViewMatrix();
            CameraUniformData[1].Data = MainCamera.GetProjectionMatrix();

            CameraUniform.BindDataMatrices([CameraUniformData[0], CameraUniformData[1]]);

            // Debug Only
#if DEBUG
            LightUniformData[0].Data = MainCamera.Position;
#endif
            LightUniformData[1].Data = MainCamera.Position;

            LightUniform.BindDataVectors([LightUniformData[0], LightUniformData[1]]);
        }

        public virtual void OnUpdateFrame(FrameEventArgs frame)
        {
           
        }

        public virtual void ListenMouseState(MouseState mouse)
        {
            if (_firstMove)
            {
                _lastPosition = new(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                Vector2 deltaPosition = new(mouse.X - _lastPosition.X, mouse.Y - _lastPosition.Y);
                _lastPosition = new Vector2(mouse.X, mouse.Y);

                MainCamera.Pitch += -deltaPosition.Y * _sensitivity;
                MainCamera.Yaw -= -deltaPosition.X * _sensitivity;
            }
        }

        public virtual CursorState MouseCursorState()
        {
            if(_freeMouse)
            {
                return CursorState.Hidden;
            }

            return CursorState.Grabbed;
        }

        public virtual void ListenMouseWheelEvent(MouseWheelEventArgs args)
        {
            MainCamera.FOV -= args.OffsetY;
        }

        public virtual void ListenKeyboardInput(KeyboardState inputKey)
        {
            if (inputKey.IsKeyDown(Keys.W))
            {
                MainCamera.Position += MainCamera.Front * _speed * (float)_deltaTime;
            }

            if (inputKey.IsKeyDown(Keys.S))
            {
                MainCamera.Position -= MainCamera.Front * _speed * (float)_deltaTime;
            }

            if (inputKey.IsKeyDown(Keys.A))
            {
                MainCamera.Position -= MainCamera.Right * _speed * (float)_deltaTime;
            }

            if (inputKey.IsKeyDown(Keys.D))
            {
                MainCamera.Position += MainCamera.Right * _speed * (float)_deltaTime;
            }

            if (inputKey.IsKeyDown(Keys.Space))
            {
                MainCamera.Position += MainCamera.Front * _speed * (float)_deltaTime;
            }

            if (inputKey.IsKeyDown(Keys.LeftShift))
            {
                MainCamera.Position -= MainCamera.Front * _speed * (float)_deltaTime;
            }

            if(inputKey.IsKeyDown(Keys.LeftAlt))
            {
                _freeMouse = !_freeMouse;
            }
        }

        public virtual void OnWindowResized(float ratio)
        {
            MainCamera.AspectRatio = ratio;
        }

        protected virtual void Release()
        {
            CameraUniform.Dispose();
            LightUniform.Dispose();
        }
        
        public void Dispose()
        {
            Release();
        }


    }
}
