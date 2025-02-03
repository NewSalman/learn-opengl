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

        private readonly float[] vertices =
        {
            // backface
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,    1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,    1.0f, 1.0f,

             0.5f,  0.5f, -0.5f,    1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,    0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,


            // front face
            -0.5f, -0.5f,  0.5f,    0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,    1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,    1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,    1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,    0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,    0.0f, 0.0f,

            // left face
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
            -0.5f, -0.5f, 0.5f,     1.0f, 0.0f,
            -0.5f, 0.5f, 0.5f,      1.0f, 1.0f,

            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
            -0.5f, 0.5f, -0.5f,     0.0f, 1.0f,
            -0.5f, 0.5f, 0.5f,      1.0f, 1.0f,

            //right face
            0.5f, -0.5f, -0.5f,     1.0f, 0.0f,
            0.5f, -0.5f, 0.5f,      0.0f, 0.0f,
            0.5f, 0.5f, 0.5f,       0.0f, 1.0f,

            0.5f, 0.5f, 0.5f,       0.0f, 1.0f,
            0.5f, 0.5f, -0.5f,      1.0f, 1.0f,
            0.5f, -0.5f, -0.5f,     1.0f, 0.0f,

            // top face
            -0.5f, 0.5f, 0.5f,      0.0f, 0.0f,
            -0.5f, 0.5f, -0.5f,     0.0f, 1.0f,
            0.5f, 0.5f, -0.5f,      1.0f, 1.0f,

            0.5f, 0.5f, -0.5f,      1.0f, 1.0f,
            0.5f, 0.5f, 0.5f,       1.0f, 0.0f,
            -0.5f, 0.5f, 0.5f,      0.0f, 0.0f,

            // bottom face
            -0.5f, -0.5f, 0.5f,     0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,    0.0f, 0.0f,
            0.5f, -0.5f, -0.5f,     1.0f, 0.0f,

            0.5f, -0.5f, -0.5f,     1.0f, 0.0f,
            0.5f, -0.5f, 0.5f,      1.0f, 1.0f,
            -0.5f, -0.5f, 0.5f,     0.0f, 1.0f

        };

       

        private readonly uint[] indices =
        {
            // front
            0, 2, 1,
            1, 3, 0,
        };

        private Shader Shader;

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private int MaxVertexAttrib = 0;
    
        private Texture _texture;
        private Texture _texture1;

        private float speed = 0.1f;
        private double _time = 0.0;

        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPosition;

        private float _sensitivity = 0.2f;

        private Mesh CubeMesh;

        private Mesh LightningSource;
        private Shader LightningShader;

        private int Ubo { get; set; }
        private int UboSize {  get; set; }


        protected override void OnLoad()
        {
            base.OnLoad();

            if (!Stopwatch.IsHighResolution) throw new Exception("Not using high resolution timer");

            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

            Ubo = GL.GenBuffer();
            UboSize = 2 * (sizeof(float) * (4 * 4));

            GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);

            // the calculation of matrix 4 by 4 is
            // calculate the amount of data on a matrix 4 by 4
            // 4 X 4 = 16, and the sizeof(float) is 4
            // so sizeof(float) X 4 X 4

            // the data will update on the loop
            // because of that the buffer usage hint is dynamic draw
            // cuz data changes every time
            GL.BufferData(BufferTarget.UniformBuffer, UboSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);




            //The function GL.VertexAttribPointer has quite a few parameters, so let's carefully walk through them:

            // 1. The first parameter specifies which vertex attribute we want to configure.Remember that we specified
            // the location of the position vertex attribute in the vertex shader with layout(location = 0).
            // This sets the location of the vertex attribute to 0 and since we want to pass data to this vertex attribute,
            // we pass in 0.

            // 2. The next argument specifies the size of the vertex attribute. The vertex attribute is a vec3 so it is composed of 3 values.

            // 3. The third argument specifies the type of the data, which is float(a vec* in GLSL consists of floating point values).

            // 4. The next argument specifies if we want the data to be normalized. If we're inputting integer data types (int, byte)
            // and we've set this to true, the integer data is normalized to 0(or - 1 for signed data) and 1 when converted to float.
            // This is not relevant for us, so we'll leave this as false.

            // 5. The fifth argument is known as the stride and tells us the space between consecutive vertex attributes.
            // Since the next set of position data is located exactly 3 times the size of a float away we specify that value as the stride.
            // Note that since we know that the array is tightly packed (there is no space between the next vertex attribute value)
            // we could've also specified the stride as 0 to let OpenGL determine the stride (this only works when values are tightly packed).
            // Whenever we have more vertex attributes we have to carefully define the spacing between each
            // vertex attribute but we'll get to see more examples of that later on.

            // 1. The position data is stored as 32 - bit(4 byte) floating point values.
            // 2. Each position is composed of 3 of those values.
            // 3. There is no space (or other values) between each set of 3 values.The values are tightly packed in the array.
            // 4. The first value in the data is at the beginning of the buffer.

            // 6. The last parameter is the offset of where the position data begins in the buffer.
            // Since the position data is at the start of the data array this value is just 0.
            // We will explore this parameter in more detail later on

            // this for applying color to object
            //GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            //GL.EnableVertexAttribArray(1);

            //{ 
            //     VertexArrayObject = GL.GenVertexArray();
            //     GL.BindVertexArray(VertexArrayObject);

            //     // Create a buffer
            //     // if Vertex the type the buffer tis BufferTarget.ArrayBuffer
            //     VertexBufferObject = GL.GenBuffer();

            //     // Bind the current Vertex Buffer Object
            //     GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            //     // Copies the previously define data vertex data into the buffer memory
            //     GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //     // chechk max vertex array attrib
            //     // my gpu currently only support up to 16
            //     //GL.GetInteger(GetPName.MaxVertexAttribs, out MaxVertexAttrib);
            //     //Console.WriteLine("Max Vertex Array Attrib: {0}", MaxVertexAttrib);

            //     ElementBufferObject = GL.GenBuffer();
            //     GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            //     GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            // }
            LightningShader = new BasicColorShader("basic/basic.vert", "basic/basic.frag");
            LightningSource = new Lightning(
                vertecies: [
                    new([-0.5f, -0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.9f)),
                    new([0.5f, -0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),
                    new([0.5f, 0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),
                    new([-0.5f, 0.5f, 0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),

                    new([-0.5f, -0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),
                    new([0.5f, -0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),
                    new([0.5f, 0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),
                    new([-0.5f, 0.5f, -0.5f], normals: [1.0f, 1.0f, 1.0f], color: new Vector3(0.9f, 0.5f, 0.4f)),
                ],
                indices: [
                        // back
                        0, 1, 2,
                        2, 3, 0,

                        //// front
                        //4, 5, 6,
                        //6, 7, 4,
                    ],
                shader: LightningShader,
                [new(1.0f, 5.0f, 1.0f)]
            );

            // Load Texture
            _texture = new("Assets/Textures/container.jpg");

            _texture1 = new("Assets/Textures/awesomeface.png");


            Shader = new TextureShader("shader.vert", "shader.frag", [_texture, _texture1]);

            CubeMesh = new Cube([
                    // rear face
                    new([-0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),

                    new([0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),


                    // front face
                    new([-0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),

                    new([0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),


                    // left face
                    new([-0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                    new([-0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),
                    new([-0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),

                    new([-0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                
                    // right face
                    new([0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),

                    new([0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),
                    new([0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),
                    new([0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),

                    // top face
                    new([-0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                    new([0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),
                    new([0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),

                    new([0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),
                    new([-0.5f, 0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),
                    new([-0.5f, 0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                
                    // bottm face
                    new([-0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                    new([0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [1.0f, 0.0f]),
                    new([0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),

                    new([0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [1.0f, 1.0f]),
                    new([-0.5f, -0.5f, -0.5f], [0f, 0f, 0f], [0.0f, 1.0f]),
                    new([-0.5f, -0.5f, 0.5f], [0f, 0f, 0f], [0.0f, 0.0f]),
                ], indices, Shader,
                [
                    new(0.0f, 0.0f, 0.0f),
                    new(2.0f, 5.0f, -15.0f),
                    new(-1.5f, -2.2f, -2.5f),
                    new(-3.8f, -2.0f, -12.5f),
                    new(2.4f, -0.4f, -3.5f),
                    new(-1.7f, 3.0f, -7.5f),
                    new(1.3f, -2.0f, -2.5f),
                    new(1.5f, 2.0f, -2.5f),
                    new(1.5f, 0.2f, -1.5f),
                    new(-1.3f, 1.0f, -1.5f)
                ]);


            //{
            //    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //    GL.EnableVertexAttribArray(0);

            //    // this for applying texture to object
            //    //int texCoord = GL.GetAttribLocation(Shader.Handle, "aTexCoord");
            //    GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof (float), 3 * sizeof(float));
            //    GL.EnableVertexAttribArray(1);
            //}

            //Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(-90.0f));
            //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);

            //Matrix4 trans = rotation * scale;

            //Shader.SetMatrix4("transform", ref trans);
            //float totalDegres = (float)MathHelper.DegreesToRadians(0.0f);
            //_model = Matrix4.CreateRotationX(totalDegres);
            //Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)args.Width / (float)args.Height, 0.1f, 100.0f);
            //Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, 800.0f, 0.0f, 600.0f, 0.1f, 100.0f);
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)args.Width / (float)args.Height, 0.1f, 100.0f);



            GL.UseProgram(Shader.Handle);
            GL.UseProgram(LightningShader.Handle);

            _camera = new Camera(Vector3.UnitZ * 3, (float)(Size.X / Size.Y));

            GL.Enable(EnableCap.DepthTest);

            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;


        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _time += 4.0 * args.Time;

            // can be use anywhere, i'm just following the tutorial
            //double elapsedTime = _stopwatch.Elapsed.Seconds;
            //float greenValue = (float)Math.Sin(elapsedTime) / 2.0f + 0.5f;
            //int vertexColorLocation = GL.GetUniformLocation(Shader.Handle, "ourColor");

            //GL.Uniform4(vertexColorLocation, new Color4(0.0f, greenValue, 0.0f, 1.0f));

            // draw outlines
            //GL.DrawElements(PrimitiveType.LineStrip, indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 projection = _camera.GetProjectionMatrix();

            // =========== try this later =============
            //unsafe
            //{
            //    Matrix4* view = _camera.GetViewMatrix();
            //    Matrix4* projection = _camera.GetProjectionMatrix();
            //}

            //LightningSource.Render(_time);

            CubeMesh.Render(_time);

            //GCHandle viewPtr = GCHandle.Alloc(view, GCHandleType.Pinned);
            //GCHandle projectionPtr = GCHandle.Alloc(projection, GCHandleType.Pinned);

            Shader.SetMatrix4("view", _camera.GetViewMatrix());
            Shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            //Matrix4 viewProjection = view *= projection;

            //// order like on the shaders
            //// for view
            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferSubData(BufferTarget.UniformBuffer, 0, sizeof(float) * 4 * 4, viewPtr.AddrOfPinnedObject());
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);
            
            //// for projection
            //GL.BindBuffer(BufferTarget.UniformBuffer, Ubo);
            //GL.BufferSubData(BufferTarget.UniformBuffer, 0, sizeof(float) * 4 * 4, projectionPtr.AddrOfPinnedObject());
            //GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            //GL.BindBufferBase(BufferRangeTarget.UniformBuffer, Constants.CameraUniformBufferPoint, Ubo);


            
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            ListenInputKey((float)_time / 1000);
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
            GL.DeleteBuffer(Ubo);

            LightningSource.Dispose();
            CubeMesh.Dispose();

            GL.DeleteProgram(0);

            base.OnUnload();
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
