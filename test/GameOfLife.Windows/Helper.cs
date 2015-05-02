namespace GameOfLife
{
    using SharpDX;
    
    public class Helper
    {
        public static Vector4 FromRGBA(byte r, byte g, byte b)
        {
            return new Vector4((float)r / 256, (float)g / 256, (float)b / 256, 1.0f);
        }
    }
}
