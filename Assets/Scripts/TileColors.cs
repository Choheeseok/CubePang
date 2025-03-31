using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class TileColors
{
    public static KeyValuePair<CustomVariables.COLOR, Color> RandomColor(uint range)
    {
        uint r = (uint)Random.Range(0, range);
        return new KeyValuePair<CustomVariables.COLOR, Color>(
            (CustomVariables.COLOR)(int)r,
            CustomVariables.Colors[(CustomVariables.COLOR)(int)r]);
    }
} 