using MyDailyLife.Constants;
using MyDailyLife.Meshes;
using MyDailyLife.Scenes.Objects.Structure;
using MyDailyLife.Shaders;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes
{
    public abstract class Drawable : IDisposable
    {
        private Shader _shader { get; set; }
        private Mesh _mesh { get; set; }
        private List<Structure> _objectStructure { get; set; }


        private VertexBuffer _buffer { get; set; }
        protected VertexBuffer Buffer => _buffer;
        
        public Drawable()
        {
            _objectStructure = AddObjectStructure();
            _buffer = CreateBuffer(_objectStructure);

            _buffer.Data = MergeBuffer();

            _mesh = CreateMesh(Buffer);
            _shader = CreateShader();

            InitializeUBOBinding(_shader);
            AfterObjectCreated();
        }

        protected virtual void InitializeUBOBinding(Shader shader) 
        {
            _shader!.Use();

            int clippedBlockIndex = _shader.GetUniformBlockIndex(UBO.CameraBlockKey);

            _shader.SetUniformBlockBinding(clippedBlockIndex, UBO.CameraBlockPoint);


            //int lightBlockIndex = shader.GetUniformBlockIndex(UBO.LightPositionBlockKey);
            //shader.SetUniformBlockBinding(lightBlockIndex, UBO.LightPositionBlockPoint);
        }

        protected virtual void AfterObjectCreated()
        {

        }

        abstract protected Shader CreateShader();
        abstract protected Mesh CreateMesh(VertexBuffer buffer);
        abstract protected VertexBuffer CreateBuffer(List<Structure> objectsStructure);
        abstract protected void Draw(double deltatime);
        abstract protected List<Structure> AddObjectStructure();
        abstract protected List<float> MergeBuffer();

        protected void SetMatrix(string name, Matrix4 data)
        {
            _shader.SetMatrix4(name, data);
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

        public virtual void Render(double deltaTime)
        {
            _mesh.OnVertexArrayBinded(() =>
            {
                _shader!.Use();
                Draw(deltaTime);
            });

        }

        protected List<float> Vec3ToFloatArr(List<Vector3> vectors)
        {
            List<float> result = new();

            for (int i = 0, k = 0; i < vectors.Count; i++, k += 3)
            {
                Vector3 current = vectors[i];

                result.AddRange([current.X, current.Y, current.Z]);
            }

            return result;

        }

        public void Dispose()
        {
            _shader.Dispose();
            _mesh.Dispose();
        }
    }
}
