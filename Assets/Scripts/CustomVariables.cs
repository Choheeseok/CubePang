using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CustomVariables
{
    public enum TURN { PLAYER_TURN, EVENT_TURN, PREPARE_TURN }
    public enum TILE { EMPTY, COLOR_BOMB, LINE_BOMB, SIDE_BOMB}
    public enum SIDE { PX, NX, PY, NY, PZ, NZ };
    public enum COLOR  { RED, GREEN, BLUE, YELLOW, CYAN, MAGENTA}

    public static readonly Dictionary<COLOR, Color> Colors = new Dictionary<COLOR, Color>()
    {
        {COLOR.RED, Color.red},
        {COLOR.GREEN, Color.green},
        {COLOR.BLUE, Color.blue},
        {COLOR.YELLOW, Color.yellow},
        {COLOR.CYAN, Color.cyan},
        {COLOR.MAGENTA, Color.magenta},
    };
    
    public readonly static float EPSILON = 0.001f;

    public const int LEFT = 0;
    public const int BACK = 1;
    public const int RIGHT = 2;
    public const int FORWORD = 3;

    public const int CAMERA_ZOOMIN = 8;
    public const int CAMERA_ZOOMOUT = 30;
    public const int CAMERA_ZOOMDEFAULT = 22;

    public static bool IsEqual(float a, float b)
    {
        if (a > b + EPSILON)
            return false;
        if (a < b - EPSILON)
            return false;
        return true;
    }

    public static bool IsEqual(Vector3 a, Vector3 b)
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

    public static KeyValuePair<CustomVariables.COLOR, Color> RandomColor(uint range)
    {
        uint r = (uint)Random.Range(0, range);
        KeyValuePair<CustomVariables.COLOR, Color> pair = 
            new KeyValuePair<CustomVariables.COLOR, Color>(
            (CustomVariables.COLOR) (int) r,
            CustomVariables.Colors[(CustomVariables.COLOR)(int) r]);
        return pair;
    }
}