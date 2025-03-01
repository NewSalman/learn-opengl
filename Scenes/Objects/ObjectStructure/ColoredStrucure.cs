using OpenTK.Mathematics;

namespace MyDailyLife.Scenes.Objects.ObjectStructure
{
    abstract public class ColoredStrucure : Structure
    {
        List<Vector3> Colors { get; set; }
        protected ColoredStrucure(Matrix4 model) : base(model)
        {
            Colors = AddColors();

            if (Colors.Count < 1 || (Colors.Count != Vertices.Count)) throw new ArgumentNullException("Colors cannot be null or Colors count must be equal");
        }

        abstract protected List<Vector3> AddColors();

        public override float[] MergedBuffer()
        {
            List<float> buffer = [];

            for (int j = 0; j < Vertices.Count; j++)
            {
                Vector3 position = Vertices[j];
                Vector3 normals = Normals[j];
                Vector3 colors = Colors[j];
                buffer.AddRange([
                    position[0],
                    position[1],
                    position[2]]);
                buffer.AddRange([normals[0], normals[1], normals[2]]);
                buffer.AddRange([colors[0], colors[1], colors[2]]);
            }

            return buffer.ToArray();
        }
    }
}
