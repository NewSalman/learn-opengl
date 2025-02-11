using OpenTK.Mathematics;

//namespace MyDailyLife.Meshes
//{
//    public class Vertex
//    {
//        public float[] Vertecies { get; set; }

//        public float[] TextureCoordinate { get; set; }

//        public float[] Normals { get; set; }

//        public Vector3 Color { get; set; }

//        private float[] MergedArrays { get; set; }

//        public int Size 
//        { 
//            get { return MergedArrays.Length; } 
//        }


//        public Vertex(float[] vetecies, float[] normals, float[] textureCoordinate) 
//        { 
//            Vertecies = vetecies;
//            TextureCoordinate = textureCoordinate;
//            Normals = normals;

//            MergedArrays = new float[8];

//            Vertecies.CopyTo(MergedArrays, 0);
//            Normals.CopyTo(MergedArrays, 3);
//            TextureCoordinate.CopyTo(MergedArrays, 6);
//        }

//        public Vertex(float[] vertecies, float[]? normals = null, Vector3? color = null)
//        {
//            Vertecies = vertecies;
//            Normals = normals ?? [0,0f, 0,0f, 0,0f];
//            Color = color ?? new(1.0f, 1.0f, 1.0f);
//            TextureCoordinate = [];

//            float[] colorArr = [Color.X, Color.Y, Color.Z];

//            MergedArrays = new float[9];

//            vertecies.CopyTo(MergedArrays, 0);
//            Normals.CopyTo(MergedArrays, 3);
//            colorArr.CopyTo(MergedArrays, 6);

//        }

//        public float[] MergeVertex()
//        {
//            return MergedArrays;

//        }

//    }

//}
