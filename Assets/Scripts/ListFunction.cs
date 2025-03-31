using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ListFunction
{
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < 50; ++i)
        {
            int idx1 = Random.Range(0, list.Count);
            int idx2 = Random.Range(0, list.Count);

            T tmp = list[idx1];
            list[idx1] = list[idx2];
            list[idx2] = tmp;
        }
    }

    public static T Pop<T>(this List<T> list)
    {
        T returnValue = list[0];
        list.RemoveAt(0);
        return returnValue;
    }
} 