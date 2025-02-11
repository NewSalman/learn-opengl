using MyDailyLife.Meshes;
using MyDailyLife.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyDailyLife.Objects
{
    public class Circle
    {
        public Vector3[] PreVertecies {  get; set; }
        public Vector3[] Vertecies {  get; set; }

        private int SectorCount {  get; set; }
        private float Radius { get; set; }
        
        public Circle() { }
        
        public Circle(int sectorCount, float radius)
        {
            SectorCount = sectorCount;
            Radius = radius;

            PreVertecies = new Vector3[SectorCount];
            Vertecies = new Vector3[SectorCount];

            GenerateCircleCylinder();
        }

        public void GenerateCircleCylinder()
        {
            float offsetZ = 0.0f;

            for (int i = 0; i < SectorCount; i++)
            {

                float angle = 2.0f * MathHelper.Pi * i / SectorCount;
                float offsetX = (float)MathHelper.Cos(angle);
                float offsetY = (float)MathHelper.Sin(angle);

                Vertecies[i] = new(offsetX, offsetY, offsetZ);
                PreVertecies[i] = new(offsetX * Radius, offsetY * Radius, offsetZ);
            }
        }
    }
}
