using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDailyLife.Constants;
using MyDailyLife.Material;
using MyDailyLife.Meshes;
using MyDailyLife.Shaders;
using OpenTK.Mathematics;

namespace MyDailyLife.Scenes
{
    public abstract class Drawable : IDisposable
    {
        private Shader? _shader { get; set; }
        private Mesh? _mesh { get; set; }
        protected Matrix4[] Models { get; set; } = [];
        protected Geometry Geometry { get; set; }
        
        protected void Initialize()
        {
            Geometry = CreateGeometry();
            _mesh = CreateMesh(Geometry);
            _shader = CreateShader();
            Models = CreateModels();

            _mesh.OnDraw = OnDraw;

            InitializeUBOBinding(_shader);
            AfterObjectCreated();
        }

        protected virtual void InitializeUBOBinding(Shader shader) 
        {
            _shader!.Use();

            int clippedBlockIndex = _shader.GetUniformBlockIndex(UBO.CameraBlockKey);

            _shader.SetUniformBlockBinding(clippedBlockIndex, UBO.CameraBlockPoint);
        }

        protected virtual void AfterObjectCreated()
        {

        }

        abstract protected Matrix4[] CreateModels();
        abstract protected Shader CreateShader();
        abstract protected void OnDraw(double DeltaTime);
        abstract protected Mesh CreateMesh(Geometry geometry);
        abstract protected Geometry CreateGeometry();
        protected void SetMatrix(string name, Matrix4 data)
        {
            _shader!.SetMatrix4(name, data);
        }


        protected void SetVector3(string name, Vector3 data)
        {
            _shader!.SetVec3(name, data);
        }

        protected void SetFloat(string name, float data)
        {
            _shader!.SetFloat(name, data);
        }

        protected void SetVectors3(Dictionary<string, Vector3> data)
        {
            _shader!.SetVectors3(data);
        }

        protected void SetInt(string name, int value)
        {
            _shader!.SetInt(name, value);
        }

        protected void UseProgram(Action<Shader> action)
        {
            _shader!.Use();
            action(_shader!);
        }

        public void Render(double deltaTime)
        {
            if (_mesh == null || _shader == null)
            {
                throw new NullReferenceException("value not instanciated, make sure call initialize is called");
            }
            _shader!.Use();
            _mesh.Render(deltaTime);
        }

        public void Dispose()
        {
            _mesh!.Dispose();
        }
    }
     public struct Geometry
    {
        public float[] Buffer;
        public uint[] Indices;
        public BufferBinding BufferBinding;
    }
}
