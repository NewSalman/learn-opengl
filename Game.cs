using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using MyDailyLife.Shaders;
using MyDailyLife.Meshes;
using MyDailyLife.Objects;
using System.Runtime.InteropServices;
using ErrorCode = OpenTK.Graphics.OpenGL4.ErrorCode;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace MyDailyLife
{
    public struct GameArgs
    {
        public int Width;
        public int Height;
        public string Title;
    }
    public class Game(GameArgs args) : GameWindow(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (args.Width, args.Height), Title = args.Title })
    {
    
        //private Texture _texture;
        //private Texture _texture1;

        private float speed = 1.0f;
        private double _time = 0.0;
        private double _lastime = 0.0;
        private double _deltaTime = 0.0;

        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPosition;

        private float _sensitivity = 0.5f;

        //private Mesh LightningSource;
        private Shader LightningShader;

        private Mesh LightCubeMesh;
        private Shader LightCubeShader;

        //private Shader Shader;
        private Mesh CubeMesh;

        private Vector3 LightPos = new(1.2f, 1.0f, 2.0f);

        private int Ubo { get; set; }
        private int UboSize {  get; set; }

        private float LightAngle = 0.0f;

        private Dictionary<string, Matrix4> WorldProjection = new();

        private Dictionary<string, Vector3> LightningValue = new();

        private Matrix4 LightCubeModel;


        private Vector3 UpdateLightPosition()
        {
            LightAngle += 4.0f * (float)_deltaTime * speed;

            if(LightAngle > 360.0f)
            {
                LightAngle -= 360.0f;
            }

            float orbitRadius = 2.0f;

            float radius = MathHelper.DegreesToRadians(LightAngle);

            float lightX = orbitRadius * (float)MathHelper.Cos(radius);
            float lightZ = orbitRadius * (float)MathHelper.Sin(radius);

            float lightY = 1.0f;

            return new Vector3(lightX, lightY, lightZ);

        }


        protected override void OnLoad()
        {
            base.OnLoad();
            

            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            Vector3 sourceColor = new(1.0f, 1.0f, 1.0f);

            uint[] ligthingIndices = [
                        // back
                        0, 1, 2,
                        2, 3, 0,

                        // front
                        4, 5, 6,
                        6, 7, 4,

                        // left
                        0, 4, 7,
                        7, 3, 0,

                        // right
                        1, 5, 6,
                        6, 2, 1,

                        // top
                        3, 7, 6,
                        6, 2, 3,

                        0, 4, 5,
                        5, 1, 0

                    ];

            Vertex[] lightningVertecies = [
                    new([-0.5f, -0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
                    new([0.5f, -0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
                    new([0.5f, 0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
                    new([-0.5f, 0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),

                    new([-0.5f, -0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
                    new([0.5f, -0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
                    new([0.5f, 0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
                    new([-0.5f, 0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: sourceColor),
            ];


            Vertex[] cubeVertecies = [
                 // rear face
                    new([-0.5f, -0.5f, -0.5f], [.0f, 0.0f, -1.0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, -0.5f], [0.0f, 0.0f, -1.0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, -0.5f], [0.0f, 0.0f, -1.0f], [1.0f, 1.0f]),

                    new([0.5f, 0.5f, -0.5f], [0.0f, 0.0f, -1.0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, -0.5f], [0.0f, 0.0f, -1.0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, -0.5f], [0.0f, 0.0f, -1.0f], [0.0f, 0.0f]),


                    // front face
                    new([-0.5f, -0.5f, 0.5f], [0.0f, 0.0f, 1.0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, 0.5f], [0.0f, 0.0f, 1.0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, 0.5f], [0.0f, 0.0f, 1.0f], [1.0f, 1.0f]),

                    new([0.5f, 0.5f, 0.5f], [0.0f, 0.0f, 1.0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, 0.5f], [0.0f, 0.0f, 1.0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, 0.5f], [0.0f, 0.0f, 1.0f], [0.0f, 0.0f]),


                    // left face
                    new([-0.5f, -0.5f, -0.5f], [-1.0f, 0.0f, 0.0f], [0.0f, 0.0f]),
                    new([-0.5f, -0.5f, 0.5f], [-1.0f, 0.0f, 0.0f], [1.0f, 0.0f]),
                    new([-0.5f, 0.5f, 0.5f], [-1.0f, 0.0f, 0.0f], [1.0f, 1.0f]),

                    new([-0.5f, 0.5f, 0.5f], [-1.0f, 0.0f, 0.0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, -0.5f], [-1.0f, 0.0f, 0.0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, -0.5f], [-1.0f, 0.0f, 0.0f], [0.0f, 0.0f]),
                
                    // right face
                    new([0.5f, -0.5f, 0.5f], [1.0f, 0.0f, 0.0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, -0.5f], [1.0f, 0.0f, 0.0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, 0.5f], [1.0f, 0.0f, 0.0f], [0.0f, 1.0f]),

                    new([0.5f, 0.5f, 0.5f], [1.0f, 0.0f, 0.0f], [0.0f, 1.0f]),
                    new([0.5f, 0.5f, -0.5f], [1.0f, 0.0f, 0.0f], [1.0f, 1.0f]),
                    new([0.5f, -0.5f, -0.5f], [1.0f, 0.0f, 0.0f], [1.0f, 0.0f]),

                    // top face
                    new([-0.5f, 0.5f, 0.5f], [0.0f, 1.0f, 0.0f], [0.0f, 0.0f]),
                    new([0.5f, 0.5f, 0.5f], [0.0f, 1.0f, 0.0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, -0.5f], [0.0f, 1.0f, 0.0f], [1.0f, 1.0f]),

                    new([0.5f, 0.5f, -0.5f], [0.0f, 1.0f, 0.0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, -0.5f], [0.0f, 1.0f, 0.0f], [0.0f, 1.0f]),
                    new([-0.5f, 0.5f, 0.5f], [0.0f, 1.0f, 0.0f], [0.0f, 0.0f]),
                
                    // bottom face
                    new([-0.5f, -0.5f, 0.5f], [0.0f, -1.0f, 0.0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, 0.5f], [0.0f, -1.0f, 0.0f], [1.0f, 0.0f]),
                    new([0.5f, -0.5f, -0.5f], [0.0f, -1.0f, 0.0f], [1.0f, 1.0f]),

                    new([0.5f, -0.5f, -0.5f], [0.0f, -1.0f, 0.0f], [1.0f, 1.0f]),
                    new([-0.5f, -0.5f, -0.5f], [0.0f, -1.0f, 0.0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, 0.5f], [0.0f, -1.0f, 0.0f], [0.0f, 0.0f]),
            ];

            uint[] cubeindices =
            {
                // front
                0, 2, 1,
                1, 3, 0,
            };



            // translate the light position and scale it down

            LightCubeModel = Matrix4.Identity;

            LightCubeModel = Matrix4.CreateTranslation(LightPos) * LightCubeModel;

            LightCubeModel = Matrix4.CreateScale(0.2f) * LightCubeModel;

            LightCubeShader = new BasicColorShader("lightning/light_cube.vert", "lightning/light_cube.frag");
            LightCubeMesh = new Cube(
                cubeVertecies,
                cubeindices,
                LightCubeShader,
                models: [LightCubeModel]
            );




            //LightningSource = new CubeEBO(
            //    vertecies: lightningVertecies,
            //    indices: ligthingIndices,
            //    shader: LightningShader,
            //    [new(0.0f, 0.0f, 0.0f)]
            //);

            //// Load Texture
            //_texture = new("Assets/Textures/container.jpg");

            //_texture1 = new("Assets/Textures/awesomeface.png");


            //Shader = new TextureShader("shader.vert", "shader.frag", [_texture, _texture1]);

            //Matrix4 LightModel = Matrix4.CreateRotationY(totalDegres);
            //Matrix4 translate = Matrix4.CreateTranslation(Positions[i]);

            //LightModel *= translate;

            LightningShader = new BasicColorShader("basic/basic.vert", "basic/basic.frag");
            CubeMesh = new Cube(cubeVertecies, cubeindices, LightningShader, [Matrix4.Identity]);

            LightningValue.Add("lightPos", UpdateLightPosition());
            LightningValue.Add("objectColor", new(1.0f, 0.5f, 0.31f));
            LightningValue.Add("lightColor", new(1.0f, 1.0f, 1.0f));

            LightningShader.SetVec3(LightningValue);




            /// ============================ try UBO again later ===================================

            //int blockIndex = GL.GetUniformBlockIndex(Shader.Handle, "Matrices");
            //GL.UniformBlockBinding(Shader.Handle, blockIndex, 0);

            //Ubo = GL.GenBuffer();
            //UboSize = 2 * (sizeof(float) * (4 * 4));

            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferData(BufferTarget.UniformBuffer, UboSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            //GL.BindBufferRange(BufferRangeTarget.UniformBuffer, blockIndex, Ubo, 0, UboSize);

            _camera = new Camera(Vector3.UnitZ * 3, (float)(Size.X / Size.Y));


            WorldProjection.Add("view", _camera.GetViewMatrix());
            WorldProjection.Add("projection", _camera.GetProjectionMatrix());

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

            Matrix4 viewMatrix = _camera.GetViewMatrix();
            Matrix4 projectionMatrix = _camera.GetProjectionMatrix();

            Vector3 lightPosition = UpdateLightPosition();

            WorldProjection["view"] = viewMatrix;
            WorldProjection["projection"] = projectionMatrix;

            LightningShader.SetVec3("lightPos", lightPosition);
            LightningShader.SetVec3("viewPos", _camera.Position);

            //LightningShader.SetVec3(LightningValue);
            LightningShader.SetMatrix4(WorldProjection);

            CubeMesh.Render(_deltaTime);
            //LightningSource.Render(deltaTime);

            /// ============================ try UBO again later ===================================

            //float[,] projection = new float[4, 4];
            //float[,] view = new float[4, 4];

            //Matrix4 viewMat = _camera.GetViewMatrix();
            //Matrix4 projectionMat = _camera.GetProjectionMatrix();


            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        view[i, j] = viewMat[i, j];
            //        projection[i, j] = projectionMat[i, j];
            //    }
            //}

            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferSubData(BufferTarget.UniformBuffer, 0, sizeof(float) * 4 * 4, projection);
            //GL.BufferSubData(BufferTarget.UniformBuffer, (sizeof(float) * 4 * 4), sizeof(float) * 4 * 4, view);
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);



            Matrix4 lightPosModel = Matrix4.Identity;
            lightPosModel = Matrix4.CreateTranslation(lightPosition) * lightPosModel;
            lightPosModel = Matrix4.CreateScale(0.2f) * lightPosModel;

            LightCubeShader.SetMatrix4(WorldProjection);
            LightCubeShader.SetMatrix4("model", lightPosModel);
            

            LightCubeMesh.Render(_deltaTime);



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

            ListenInputKey((float)_deltaTime);
            ListenMouseEvent();
        }

        private void ListenMouseEvent()
        {
            MouseState mouse = MouseState;
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

                _camera.Pitch += -deltaPosition.Y * _sensitivity;
                _camera.Yaw -= -deltaPosition.X * _sensitivity;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.FOV -= e.OffsetY;
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
            LightCubeMesh.Dispose();
            //CubeMesh.Dispose();

            GL.DeleteProgram(0);

            base.OnUnload();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        private void ListenInputKey(float time)
        {
            if (!IsFocused) return;

            KeyboardState inputKey = KeyboardState;

            if(inputKey.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (inputKey.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * speed * time;
            }

            if (inputKey.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * speed * time;
            }

            if (inputKey.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * speed * time;
            }

            if (inputKey.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * speed * time;
            }

            if (inputKey.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * speed * time;
            }

            if (inputKey.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * speed * time;
            }
        }
    }
}
