//using MyDailyLife.Meshes;
//using MyDailyLife.Shaders;
//using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics;

//namespace MyDailyLife.Scenes.Components
//{
//    public class Light : Drawable
//    {
//        public Vector3 LightPos { get; set; }

//        public Light()
//        {
//            Initialize();
//        }

//        protected override Matrix4[] CreateModels()
//        {
//            Matrix4 model = Matrix4.CreateScale(0.2f);
//            model = model * Matrix4.CreateTranslation(LightPos);

//            return [model];
//        }

//        protected override Shader CreateShader()
//        {
//            return new BasicColorShader("lightning/light_cube.vert", "lightning/light_cube.frag");
//        }

//        protected override Mesh CreateMesh(Geometry geometry)
//        {
//            return new Mesh(geometry);
//        }

//        protected override Geometry CreateGeometry()
//        {
//            List<float> buffer = new();

//            float vertex = 0.5f;

//            buffer.AddRange([
//                -vertex, -vertex, vertex,   1.0f, 1.0f, 1.0f,
//                vertex, -vertex, vertex,    1.0f, 1.0f, 1.0f,
//                vertex, vertex, vertex,     1.0f, 1.0f, 1.0f,
//                -vertex, vertex, vertex,    1.0f, 1.0f, 1.0f,

//                -vertex, -vertex, -vertex,  1.0f, 1.0f, 1.0f,
//                vertex, -vertex, -vertex,   1.0f, 1.0f, 1.0f,
//                vertex, vertex, -vertex,    1.0f, 1.0f, 1.0f,
//                -vertex, vertex, -vertex,   1.0f, 1.0f, 1.0f
//            ]);

//            return new Geometry
//            {
//                Buffer = buffer.ToArray(),
//                Indices = 
//                [
//                    0, 1, 2,
//                    2, 3, 0,

//                    4, 5, 6,
//                    6, 7, 4,

//                    0, 4, 3,
//                    3, 7, 4,

//                    1, 5, 2,
//                    2, 7, 5,

//                    3, 7, 6,
//                    6, 3, 2,

//                    0, 4, 5,
//                    5, 1, 0,
                    
//                ],
//                BufferBinding = BufferBinding.Vertices_Colors
//            };
//        }

//        protected override void OnDraw(double deltaTime)
//        {
//            Matrix4 model = Matrix4.CreateScale(0.2f) * Matrix4.CreateTranslation(LightPos);
//            SetMatrix("model", model);

//            GL.DrawElements(PrimitiveType.Triangles, Geometry.Indices.Length, DrawElementsType.UnsignedInt, 0);
//        }
//    }
//}
