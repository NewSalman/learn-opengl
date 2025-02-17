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
        private Shader? Shader { get; set; }
        private Mesh? Mesh { get; set; }
        protected Matrix4[] Models { get; set; } = [];
        protected Geometry Geometry { get; set; }
        
        protected void Initialize()
        {
            Geometry = CreateGeometry();
            Mesh = CreateMesh(Geometry);
            Shader = CreateShader();
            Models = CreateModels();

            Mesh.OnDraw = OnDraw;

            InitializeUBOBinding(Shader);
            AfterObjectCreated();
        }

        protected virtual void InitializeUBOBinding(Shader shader) 
        {
            Shader!.Use();

            int clippedBlockIndex = Shader.GetUniformBlockIndex(UBO.CameraBlockKey);

            Shader.SetUniformBlockBinding(clippedBlockIndex, UBO.CameraBlockPoint);
        }

        protected virtual void AfterObjectCreated()
        {

        }

        abstract protected Matrix4[] CreateModels();
        abstract protected Shader CreateShader();
        abstract protected void OnDraw(double DeltaTime);
        abstract protected Mesh CreateMesh(Geometry geometry);
        abstract protected Geometry CreateGeometry();
        protected virtual void SetMatrix(string name, Matrix4 data)
        {
            Shader!.SetMatrix4(name, data);
        }

        public void Render(double deltaTime)
        {
            if (Mesh == null)
            {
                throw new NullReferenceException("value not instanciated, make sure call initialize is called");
            }

            Mesh.Render(deltaTime);
        }

        public void Dispose()
        {
            Mesh!.Dispose();
        }
    }
     public struct Geometry
    {
        public float[] Buffer;
        public uint[] Indices;
        public BufferBinding BufferBinding;
    }
}
