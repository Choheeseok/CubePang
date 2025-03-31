using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private const uint LEVEL_4 = 4;
    private const uint LEVEL_5 = 5;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text colorCountText;

    public void UpdateScore(uint score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateColorCount(Dictionary<CustomVariables.COLOR, uint> tileCountDictionary, uint level)
    {
        var colorTexts = new List<string>
        {
            FormatColorCount("Red", tileCountDictionary[CustomVariables.COLOR.RED]),
            FormatColorCount("Green", tileCountDictionary[CustomVariables.COLOR.GREEN]),
            FormatColorCount("Blue", tileCountDictionary[CustomVariables.COLOR.BLUE]),
            FormatColorCount("Yellow", tileCountDictionary[CustomVariables.COLOR.YELLOW])
        };

        if (level >= LEVEL_4)
        {
            colorTexts.Add(FormatColorCount("Cyan", tileCountDictionary[CustomVariables.COLOR.CYAN]));
        }

        if (level >= LEVEL_5)
        {
            colorTexts.Add(FormatColorCount("Magenta", tileCountDictionary[CustomVariables.COLOR.MAGENTA]));
        }

        colorCountText.text = string.Join("\n", colorTexts);
    }

    private string FormatColorCount(string colorName, uint count)
    {
        return $"{colorName} : {count}";
    }
}
