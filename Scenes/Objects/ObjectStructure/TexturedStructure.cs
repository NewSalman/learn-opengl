using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.ObjectStructure
{
    public abstract class TexturedStructure : Structure
    {
        public List<Vector2> TextureCoordinates { get; private set; }

        public TexturedStructure(Matrix4 model) : base(model)
        {
            TextureCoordinates = AddTextureCoordinate();

            if(TextureCoordinates.Count != Vertices.Count)
            {
                throw new InvalidDataException("data length or count not equal");
            }
        }
        
        abstract protected List<Vector2> AddTextureCoordinate();

        public override float[] MergedBuffer()
        {
            List<float> buffer = [];

            for (int j = 0; j < Vertices.Count; j++)
            {
                Vector3 position = Vertices[j];
                Vector3 normals = Normals[j];
                Vector2 textureCoordinate = TextureCoordinates[j];
                buffer.AddRange([
                    position[0],
                    position[1],
                    position[2]]);
                buffer.AddRange([normals[0], normals[1], normals[2]]);
                buffer.AddRange([textureCoordinate[0], textureCoordinate[1]]);
            }

            return buffer.ToArray();
        }
    }
}
