namespace MyDailyLife.Constants
{
    public static class UBO
    {
        public static int CameraBlockPoint = 0;
        public static string CameraBlockKey = "CameraBlock";

        public static int LightPositionBlockPoint = 1;
        public static string LightPositionBlockKey = "LightBlock";
    }

    public enum Texture
    {
        Ambient_Occlusion,
        Roughness,
        Metallic,
        Normal,
        Height,
        Albedo,
        ARM_Combined,
        Emissive,
        Opacity
    }
}
