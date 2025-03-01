

using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.ObjectStructure
{
    public abstract class Structure
    {
        public List<Vector3> Vertices { get; private set; }
        public List<Vector3> Normals { get; private set; }

        public List<uint> Indices { get; private set; }

        public Matrix4 Model { get; private set; }
        public Matrix3 NormalMat { get; private set; }
        public Structure(Matrix4 model)
        {
            Model = model;
            Vertices = AddVertices();
            Normals = AddNormals();
            Indices = AddIndices();

            InverseNormal();

            if((Vertices.Count < 1 && Normals.Count < 1) && Vertices.Count != Normals.Count)
            {
                throw new ArgumentNullException("data invalid, make sure data length or count are equal");
            }
        }
        abstract protected List<Vector3> AddVertices();
        abstract protected List<Vector3> AddNormals();

        protected virtual List<uint> AddIndices()
        {
            return [];
        }

        public virtual void CalculateModel(Matrix4 matrix)
        {
            Model = Model * matrix;
            InverseNormal();
        }

        public virtual void InverseNormal()
        {
            NormalMat = new Matrix3(Model.Inverted());
        }
        

        // TODO: implement later
        public virtual float[] MergedBuffer()
        {
            List<float> buffer = new();

            for (int j = 0; j < Vertices.Count; j++)
            {
                Vector3 position = Vertices[j];
                Vector3 normals = Normals[j];
                buffer.AddRange([
                    position[0],
                    position[1],
                    position[2]]);
                buffer.AddRange([normals[0], normals[1], normals[2]]);
            }

            return buffer.ToArray();
        }

    }
}
