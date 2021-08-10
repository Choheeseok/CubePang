using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    List<Tile> tileList;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text colorCountText;


    public void UpdateScore(uint score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateColorCount(Dictionary<CustomVariables.COLOR,  uint> tileCountDictionary, uint level)
    {
        string text = "";
        text += "\nRed : " + tileCountDictionary[CustomVariables.COLOR.RED].ToString();
        text += "\nGreen : " + tileCountDictionary[CustomVariables.COLOR.GREEN].ToString();
        text += "\nBlue : " + tileCountDictionary[CustomVariables.COLOR.BLUE].ToString();
        text += "\nYellow : " + tileCountDictionary[CustomVariables.COLOR.YELLOW].ToString();
        if (4 == level)
            text += "\nCyan : " + tileCountDictionary[CustomVariables.COLOR.CYAN].ToString();
        if (5 == level)
            text += "\nMagenta : " + tileCountDictionary[CustomVariables.COLOR.MAGENTA].ToString();

        colorCountText.text = text;
    }
}
