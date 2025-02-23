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

        private UniformBuffer CameraUniform { get; set; }
        private UniformBufferData<Matrix4>[] CameraBufferData = new UniformBufferData<Matrix4>[2]; 
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
            CameraBufferData[0] = new UniformBufferData<Matrix4>(CameraBufferInfo.EachSize, CameraBufferInfo.ViewOffset, MainCamera.GetViewMatrix());
            CameraBufferData[1] = new UniformBufferData<Matrix4>(CameraBufferInfo.EachSize, CameraBufferInfo.ProjectionOffset, MainCamera.GetProjectionMatrix());

        }

        public virtual void Render(double deltaTime)
        {
            _deltaTime = deltaTime;
            CameraBufferData[0].Data = MainCamera.GetViewMatrix();
            CameraBufferData[1].Data = MainCamera.GetProjectionMatrix();

            CameraUniform.BindDataMatrices([.. CameraBufferData]);
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
            //CameraUniform.Dispose();
        }
        
        public void Dispose()
        {
            Release();
        }


    }
}
