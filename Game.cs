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
        private int Ubo { get; set; }
        private int UboSize {  get; set; }

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

            //Vector3 sourceColor = new(1.0f, 1.0f, 1.0f);

            //uint[] ligthingIndices = [
            //            // back
            //            0, 1, 2,
            //            2, 3, 0,

            //            // front
            //            4, 5, 6,
            //            6, 7, 4,

            //            // left
            //            0, 4, 7,
            //            7, 3, 0,

            //            // right
            //            1, 5, 6,
            //            6, 2, 1,

            //            // top
            //            3, 7, 6,
            //            6, 2, 3,

            //            0, 4, 5,
            //            5, 1, 0

            //        ];



            //// translate the light position and scale it down

            //LightCubeModel = Matrix4.Identity;

            ////LightCubeModel = LightCubeModel * Matrix4.CreateTranslation(LightPos);

            //LightCubeModel = LightCubeModel * Matrix4.CreateScale(0.2f);

            //LightCubeShader = new BasicColorShader("lightning/light_cube.vert", "lightning/light_cube.frag");
            //LightCubeMesh = new Cube(
            //    cubeVertecies,
            //    cubeindices,
            //    LightCubeShader,
            //    models: [LightCubeModel]
            //);




            ////LightningSource = new CubeEBO(
            ////    vertecies: lightningVertecies,
            ////    indices: ligthingIndices,
            ////    shader: LightningShader,
            ////    [new(0.0f, 0.0f, 0.0f)]
            ////);

            ////// Load Texture
            ////_texture = new("Assets/Textures/container.jpg");

            ////_texture1 = new("Assets/Textures/awesomeface.png");


            ////Shader = new TextureShader("shader.vert", "shader.frag", [_texture, _texture1]);

            ////Matrix4 LightModel = Matrix4.CreateRotationY(totalDegres);
            ////Matrix4 translate = Matrix4.CreateTranslation(Positions[i]);

            ////LightModel *= translate;

            //LightningShader = new BasicColorShader("basic/basic.vert", "basic/basic.frag");
            //CubeMesh = new Cube(cubeVertecies, cubeindices, LightningShader, [Matrix4.Identity]);

            //LightningValue.Add("lightPos", UpdateLightPosition());
            ////LightningValue.Add("objectColor", new(1.0f, 0.5f, 0.31f));
            ////LightningValue.Add("lightColor", new(1.0f));
            ///
            //LightningValue.Add("material.ambient", new(1.0f, 0.5f, 0.31f));
            //LightningValue.Add("material.diffuse", new(1.0f, 0.5f, 0.31f));
            //LightningValue.Add("material.specular", new(0.5f, 0.5f, 0.5f));

            //LightningValue.Add("light.ambient", new(0.2f));
            //LightningValue.Add("light.diffuse", new(0.5f));
            //LightningValue.Add("light.specular", new(1.0f));


            //LightningShader.SetVec3(LightningValue);
            //LightningShader.SetFloat("material.shininess", 32.0f);

            Scenes.Add(worldScene);

            SelectedScene = Scenes[0];

            //Ubo = GL.GenBuffer();
            //UboSize = 2 * (sizeof(float) * (4 * 4));

            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferData(BufferTarget.UniformBuffer, UboSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            //GL.BindBufferRange(BufferRangeTarget.UniformBuffer, UBO.CameraBlockPoint, Ubo, 0, UboSize);

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

            //Matrix4 viewMatrix = _camera.GetViewMatrix();
            //Matrix4 projectionMatrix = _camera.GetProjectionMatrix();

            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferSubData(BufferTarget.UniformBuffer, 0, sizeof(float) * 4 * 4, [Matrix4.Transpose(viewMatrix)]);
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            
            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferSubData(BufferTarget.UniformBuffer, sizeof(float) * 4 * 4, sizeof(float) * 4 * 4, [Matrix4.Transpose(projectionMatrix)]);
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            SelectedScene?.Render(_deltaTime);

            //SceneObjects[0].Render(_deltaTime);

            //Vector3 lightPosition = UpdateLightPosition();

            //LightningShader.SetVec3("lightPos", lightPosition);
            //LightningShader.SetVec3("viewPos", _camera.Position);

            //CubeMesh.Render(_deltaTime);

            //Matrix4.CreateScale(0.2f); // We scale the lamp cube down a bit to make it less dominant
            //lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);

            //Matrix4 lightPosModel = Matrix4.CreateScale(0.2f);
            //lightPosModel = lightPosModel * Matrix4.CreateTranslation(lightPosition);

            //LightCubeShader.SetMatrix4("model", lightPosModel);
            //LightCubeMesh.Render(_deltaTime);


            //Cylinder.Render(_deltaTime);



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
            //Note: After the program ends, all resources used by the process is freed.This means there is no need to delete buffers
            //before closing your program.But if you want to delete buffers for other reasons like limiting VRAM usage,
            //you can do the following:

            //Shader.Dispose();
            //LightningShader.Dispose();

            //GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.BindVertexArray(0);
            //GL.UseProgram(0);

            //GL.DeleteBuffer(VertexBufferObject);
            //GL.DeleteBuffer(ElementBufferObject);
            //GL.DeleteVertexArray(VertexArrayObject);
            //GL.DeleteBuffer(Ubo);

            //LightningSource.Dispose();
            //LightCubeMesh.Dispose();
            //CubeMesh.Dispose();

            for(int i = 0; i < Scenes.Count; i++)
            {
                Scenes[i].Dispose();
            }

            GL.DeleteProgram(0);

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
