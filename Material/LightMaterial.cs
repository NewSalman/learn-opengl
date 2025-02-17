using OpenTK.Mathematics;

namespace MyDailyLife.Material
{
    public class LightMaterial
    {
        public Vector3 Position { get; set; }
        public nint PosOffset = 0;
        public Vector3 ViewPosition { get; set; }
        public nint ViewPosOffset = 3 * sizeof(float);
        public Vector3 Ambient { get; set; }
        public nint AmbientOffset = 6 * sizeof(float);
        public Vector3 Diffuse { get; set; }
        public nint ADiffuseOffset = 9 * sizeof(float);
        public Vector3 Specular { get; set; }
        public nint SpecularOffset = 12 * sizeof(float);

        public nint Size = 5 * 3 * sizeof(float);

        public LightMaterial(Vector3 position, Vector3 viewPosition, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            Position = position;
            ViewPosition = viewPosition;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }
    }
}
