namespace Baruch.Extension
{
    public static class FloatExtensions
    {
        public static bool Approximately(this float from, float b, float threshold = 0.01f)
        {
            return ((from - b) < 0 ? ((from - b) * -1) : (from - b)) <= threshold;
        }
        public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (from - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }


    }
}