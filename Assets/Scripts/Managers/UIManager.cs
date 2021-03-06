using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    int score;
    List<Tile> tileList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScore();
        UpdateColorCount();
    }

    public void UpdateScore()
    {
        // transform.GetChild(0) : Canvas
        score = GameManager.instance.Score;
        transform.GetChild(0).Find("ScoreText").GetComponent<Text>().text = score.ToString();
    }

    public void UpdateColorCount()
    {
        string text; //= transform.GetChild(0).Find("ColorCountText").GetComponent<Text>().text;
        text = "";
        tileList = GameManager.instance.TileList;
        int[] colors = new int[GameManager.instance.Level];
        foreach (Tile tile in tileList)
        {
            if (tile.IsSameColor(Color.red)) ++colors[0];
            else if (tile.IsSameColor(Color.green)) ++colors[1];
            else if (tile.IsSameColor(Color.blue)) ++colors[2];
            else if (tile.IsSameColor(Color.yellow)) ++colors[3];
            else if (tile.IsSameColor(Color.cyan)) ++colors[4];
            else if (tile.IsSameColor(Color.magenta)) ++colors[5];
        }
        for(int i = 0; i < GameManager.instance.Level; ++i)
        {
            if (0 == i) text += "\nRed : " + colors[i].ToString();
            if (1 == i) text += "\nGreen : " + colors[i].ToString();
            if (2 == i) text += "\nBlue : " + colors[i].ToString();
            if (3 == i) text += "\nYellow : " + colors[i].ToString();
            if (4 == i) text += "\nCyan : " + colors[i].ToString();
            if (5 == i) text += "\nMagenta : " + colors[i].ToString();
        }
        transform.GetChild(0).Find("ColorCountText").GetComponent<Text>().text = text;
    }
}
