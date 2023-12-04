namespace API.Core.Extensions
{
    public static class CommonExtensions
    {
        public static bool CompareEqual(this byte[] bytes, byte[] bytesToCompare)
        {
            if (bytes.Length != bytesToCompare.Length)
            {
                return false;
            }
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != bytesToCompare[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}