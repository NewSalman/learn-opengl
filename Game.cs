using OpenTK.Compute.OpenCL;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Resources;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Drawing;
using System.Reflection.Emit;
using System;
using System.Data.Common;

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
            // Position         Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private Shader Shader;

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private int MaxVertexAttrib = 0;
        private Stopwatch _stopwatch;
        private float[] textureCoord =
        {
            0.0f, 0.0f, //lower left
            1.0f, 0.0f, // lower right
            0.0f, 1.0f, // upper left
            1.0f, 1.0f  // upper right
        };
        private Texture _texture;
        private Texture _texture1;

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            //if (KeyboardState.IsKeyDown(Keys.Escape))
            //{
            //    Close();
            //}
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            _stopwatch = Stopwatch.StartNew();
            if (!Stopwatch.IsHighResolution) throw new Exception("Not using high resolution timer");

            GL.ClearColor(new Color4(0.2f, 0.3f, 0.3f, 1.0f));

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

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // Create a buffer
            // if Vertex the type the buffer tis BufferTarget.ArrayBuffer
            VertexBufferObject = GL.GenBuffer();

            // Bind the current Vertex Buffer Object
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Copies the previously define data vertex data into the buffer memory
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // chechk max vertex array attrib
            // my gpu currently only support up to 16
            //GL.GetInteger(GetPName.MaxVertexAttribs, out MaxVertexAttrib);
            //Console.WriteLine("Max Vertex Array Attrib: {0}", MaxVertexAttrib);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            Shader = new("shader.vert", "shader.frag");
            Shader.Use();


            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // this for applying texture to object
            //int texCoord = GL.GetAttribLocation(Shader.Handle, "aTexCoord");
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof (float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Load Texture
            _texture = new("Assets/Textures/container.jpg");
            _texture.Use(TextureUnit.Texture0);

            _texture1 = new("Assets/Textures/awesomeface.png");
            _texture1.Use(TextureUnit.Texture1);

            Shader.SetInt("texture0", 0);
            Shader.SetInt("texture1", 1);

            //Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(-90.0f));
            //Matrix4 scale = Matrix4.CreateScale(0.5f, 0.5f, 0.5f);

            //Matrix4 trans = rotation * scale;

            //Shader.SetMatrix4("transform", ref trans);

            //Matrix4 model = Matrix4.CreateRotationX(-55.0f);
            //Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            //Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)args.Width / (float)args.Height, 0.1f, 100.0f);

            //Shader.SetMatrix4("model", ref model);
            //Shader.SetMatrix4("view", ref view);
            //Shader.SetMatrix4("projection", ref projection);
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Shader.Use();
            _texture.Use(TextureUnit.Texture0);
            _texture1.Use(TextureUnit.Texture1);


            // can be use anywhere, i'm just following the tutorial
            //double elapsedTime = _stopwatch.Elapsed.Seconds;
            //float greenValue = (float)Math.Sin(elapsedTime) / 2.0f + 0.5f;
            //int vertexColorLocation = GL.GetUniformLocation(Shader.Handle, "ourColor");

            //GL.Uniform4(vertexColorLocation, new Color4(0.0f, greenValue, 0.0f, 1.0f));

            GL.BindVertexArray(VertexArrayObject);
            // draw outlines
            //GL.DrawElements(PrimitiveType.LineStrip, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
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

            Shader.Dispose();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);

            GL.DeleteProgram(0);

            _stopwatch.Stop();

            base.OnUnload();
        }
    }
}
