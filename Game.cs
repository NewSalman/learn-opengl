using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;
using MyDailyLife.Constants;
using MyDailyLife.Scenes.WorldScene;
using MyDailyLife.Scenes;

namespace MyDailyLife
{
    public struct GameArgs
    {
        public int Width;
        public int Height;
        public string Title;
    }
    public class Game : GameWindow
    {

        private double _time = 0.0;
        private double _lastime = 0.0;
        private double _deltaTime = 0.0;

        private float WindowRatio { get; set; }
        private GameArgs Args { get; set; }

        private readonly List<Scene> Scenes = new();
        private Scene? SelectedScene;

        public Game(GameArgs args) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (args.Width, args.Height), Title = args.Title })
        {
            Args = args;
            WindowRatio = (float)(Size.X / Size.Y);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            

            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            Scene worldScene = new WorldScene(WindowRatio);

            Scenes.Add(worldScene);

            SelectedScene = Scenes[0];

            GL.Enable(EnableCap.DepthTest);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;


        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            _lastime = _time;
            _time += args.Time;
            _deltaTime = _time - _lastime;

            SelectedScene?.Render(_deltaTime);


            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"OpenGL Error: {error}");
                // Handle the error appropriately (e.g., throw an exception)
            }

            SwapBuffers();
        }



        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            SelectedScene?.OnUpdateFrame(args);

            if (!IsFocused) return;

            KeyboardState inputKey = KeyboardState;

#if DEBUG
            if (inputKey.IsKeyDown(Keys.Escape))
            {
                Close();
            }
#endif
            SelectedScene?.ListenKeyboardInput(inputKey);

            SelectedScene?.ListenMouseState(MouseState);

            CursorState = SelectedScene?.MouseCursorState() ?? CursorState.Grabbed;
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            SelectedScene?.ListenMouseWheelEvent(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {

            for(int i = 0; i < Scenes.Count; i++)
            {
                Scenes[i].Dispose();
            }

            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            SelectedScene?.OnWindowResized(Size.X / (float)Size.Y);
        }
    }
}
