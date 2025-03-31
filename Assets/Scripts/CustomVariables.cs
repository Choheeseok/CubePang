using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GameConstants
{
    public const float EPSILON = 0.001f;
    public const int LEFT = 0;
    public const int BACK = 1;
    public const int RIGHT = 2;
    public const int FORWARD = 3;
    public const int CAMERA_ZOOMIN = 8;
    public const int CAMERA_ZOOMOUT = 30;
    public const int CAMERA_ZOOMDEFAULT = 22;
}

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
    
    public static bool IsEqual(float a, float b)
    {
        return Mathf.Abs(a - b) <= GameConstants.EPSILON;
    }

    public static bool IsEqual(Vector3 a, Vector3 b)
    {
        float dotValue = Vector3.Dot(a, b);
        return Mathf.Abs(dotValue - 1) <= GameConstants.EPSILON;
    }
}