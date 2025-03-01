using MyDailyLife.Constants;
using MyDailyLife.Meshes;
using MyDailyLife.Scenes.Objects.ObjectStructure;
using MyDailyLife.Shaders;
using OpenTK.Mathematics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyDailyLife.Scenes
{
    public abstract class Drawable<T> where T : Structure
    {
        private Shader _shader { get; set; }
        private Mesh _mesh { get; set; }
        protected List<T> DrawableObjects { get; set; }


        private VertexBuffer? _buffer { get; set; }
        protected VertexBuffer? Buffer => _buffer;


        private Matrix4 _model = Matrix4.Identity;
        public Matrix4 Model => _model;

        public Drawable(Matrix4 model)
        {
            /// TODO: still can't rotate objects as whole
            //_model = _model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f));

            DrawableObjects = AddObjectStructures();

            _buffer = MergeBuffer(DrawableObjects);
            if (_buffer == null) throw new ArgumentNullException("buffer is null");
            _mesh = CreateMesh(_buffer);
            _shader = CreateShader();

            InitializeUBOBinding(_shader);
            Initialized();
        }

        protected virtual void InitializeUBOBinding(Shader shader) 
        {
            _shader!.Use();

            int clippedBlockIndex = _shader.GetUniformBlockIndex(UBO.CameraBlockKey);
            _shader.SetUniformBlockBinding(clippedBlockIndex, UBO.CameraBlockPoint);


            int lightBlockIndex = shader.GetUniformBlockIndex(UBO.LightPositionBlockKey);
            shader.SetUniformBlockBinding(lightBlockIndex, UBO.LightPositionBlockPoint);
        }


        protected virtual void Initialized()
        {

        }

        abstract protected Shader CreateShader();
        abstract protected Mesh CreateMesh(VertexBuffer buffer);
        abstract protected void Draw(double deltatime);
        abstract protected List<T> AddObjectStructures();
        protected virtual VertexBuffer MergeBuffer(List<T> objectsStructure)
        {
            List<float> buffer = [];
            List<uint> indices = [];

            for (int i = 0; i < objectsStructure.Count; i++)
            {
                T? structure = objectsStructure[i];

                if(structure is null) throw new NullReferenceException("strucure is null");

                buffer.AddRange(structure?.MergedBuffer() ?? []);
                indices.AddRange(structure?.Indices ?? []);
            }

            Type sType = typeof(T);

            if(sType == typeof(TexturedStructure))
            {
                return new VertexBuffer(buffer, indices, [BufferType.Vertex, BufferType.Normal, BufferType.TextureCoordinate]);
            }

            if(sType == typeof(ColoredStrucure))
            {
                return new VertexBuffer(buffer, indices, [BufferType.Vertex, BufferType.Normal, BufferType.Color]);
            }

            return new VertexBuffer(buffer, indices, [BufferType.Vertex, BufferType.Normal]);
        }

        public void RotateX(float degrees)
        {
            _model = _model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(degrees));
        }
        public void RotateY(float degrees)
        {
            _model = _model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(degrees));
        }

        public void RotateZ(float degress)
        {
            _model = _model * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(degress));
        }

        protected void SetMatrix(string name, dynamic matrix)
        {
            if(matrix is Matrix4)
            {
                Matrix4 mat4 = matrix;
                _shader.SetMatrix4(name, mat4);
            }

            if(matrix is Matrix3)
            {
                Matrix3 mat3 = matrix;
                _shader.SetMatrix3(name, mat3);
            }
        }
        protected void SetVector3(string name, Vector3 data)
        {
            _shader.SetVec3(name, data);
        }

        protected void SetFloat(string name, float data)
        {
            _shader.SetFloat(name, data);
        }

        protected void SetVectors3(Dictionary<string, Vector3> data)
        {
            _shader.SetVectors3(data);
        }

        protected void SetInt(string name, int value)
        {
            _shader.SetInt(name, value);
        }

        protected void RequestProgramFocus()
        {
            _shader.Use();
        }

        public virtual void Render(double deltaTime)
        {
            _mesh.OnVertexArrayBinded(() =>
            {
                _shader!.Use();
                Draw(deltaTime);
            });

        }

        public void Dispose()
        {
            _shader.Dispose();
            _mesh.Dispose();

            _buffer = null;
            DrawableObjects = new();
        }
    }
}
