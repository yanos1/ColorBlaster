using UnityEngine;

namespace Extentions
{
    public static class UtilityFunctions
    {
        public static T[] ShuffleArray<T>(T[] array)
        {
            T[] copy = (T[])array.Clone();
            System.Random random = new System.Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (copy[i], copy[j]) = (copy[j], copy[i]);
            }

            return copy;
        }

    }
}