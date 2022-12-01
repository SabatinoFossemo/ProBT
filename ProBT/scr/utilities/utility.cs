namespace ProBT
{
    public static class utility
    {
        public static double RoundTicks(this double value, double ticksize)
        {
            var inv = 1.0 / ticksize;

            return Math.Round(value * inv) / inv;
        }
    }
}