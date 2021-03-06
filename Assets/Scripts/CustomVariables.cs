using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자 정의 상수입니다. 변하지 않는 변수는 여기다 집어넣으라구~

public class CustomVariables
{
    public enum TURN { PLAYER_TURN, EVENT_TURN, PREPARE_TURN }
    public enum TILE { EMPTY, COLOR_BOMB, LINE_BOMB, SIDE_BOMB}
    public enum SIDE { PX, NX, PY, NY, PZ, NZ };
    public enum COLOR { RED, GREEN, BLUE, YELLOW, CYAN, MAGENTA}
    public readonly static float EPSILON = 0.001f;

    public const int LEFT = 0;
    public const int BACK = 1;
    public const int RIGHT = 2;
    public const int FORWORD = 3;

    public const int CAMERA_ZOOMIN = 8;
    public const int CAMERA_ZOOMOUT = 30;
    public const int CAMERA_ZOOMDEFAULT = 22;

    public static bool IsSimillerValue(float a, float b)
    {
        if (a > b + EPSILON)
            return false;
        if (a < b - EPSILON)
            return false;
        return true;
    }

    public static bool IsSimillerVectorDir(Vector3 a, Vector3 b)
    {
        float dotValue = Vector3.Dot(a, b);
        if (dotValue > 1 + EPSILON)
            return false;
        if (dotValue < 1 - EPSILON)
            return false;
        return true;
    }
}

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
        T returnInt = list[0];

        list.RemoveAt(0);
        return returnInt;
    }
}


public static class TileColors
{
    public static Color RandomColor(int range)
    {
        int r = Random.Range(0, range);
        switch (r)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.magenta;
            default: return Color.white;    // error color
        }
    }

}