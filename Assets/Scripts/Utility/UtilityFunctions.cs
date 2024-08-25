using UnityEngine;

namespace Extentions
{
    public static class UtilityFunctions
    {
        public static void ShuffleArray<T>(T[] array)
        {
            System.Random random = new System.Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

    }
}