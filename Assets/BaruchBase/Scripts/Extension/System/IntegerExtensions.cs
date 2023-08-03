namespace Baruch.Extension
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool LessThan(this int a, int b)
        {
            return a < b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool LessThanOrEqual(this int a, int b)
        {
            return a <= b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool GreaterThan(this int a, int b)
        {
            return a > b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool GreaterThanOrEqual(this int a, int b)
        {
            return a >= b;
        }
    }
}