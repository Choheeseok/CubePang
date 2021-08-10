using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private CustomVariables.COLOR color;
    private Color materialColor;
    
    public GameObject child;
    public CustomVariables.TILE type;

    Color prevTileColor;

    public GameObject particle;
    public GameObject playParticle;

    void Awake()
    {
        playParticle = Instantiate(particle, transform);
        child = null;
        type = CustomVariables.TILE.EMPTY;
    }

    public void SetChild(GameObject child_, CustomVariables.TILE type_)
    {
        child = child_;
        type = type_;
    }

    public void SetColor(KeyValuePair<CustomVariables.COLOR, Color> pair)
    {
        GameManager.instance.TileCountDictionary[color]--;
        GameManager.instance.TileCountDictionary[pair.Key]++;
        
        color = pair.Key;
        materialColor = pair.Value;
        transform.GetComponent<MeshRenderer>().material.color = materialColor;
    }
    
    public void SaveCurTileColor()
    {
        prevTileColor = transform.GetComponent<MeshRenderer>().material.color;
    }

    public void LoadPrevTileColor()
    {
        transform.GetComponent<MeshRenderer>().material.color = prevTileColor;
    }

    public void TakeDamage()
    {
        KeyValuePair<CustomVariables.COLOR, Color> pair = TileColors.RandomColor(GameManager.instance.Level);
        GameManager.instance.TileCountDictionary[color]--;
        GameManager.instance.TileCountDictionary[pair.Key]++;
        color = pair.Key;
        materialColor = pair.Value;
        transform.GetComponent<MeshRenderer>().material.color = materialColor;
        
        playParticle.GetComponent<ParticleSystem>().Play();
        ++GameManager.instance.Score;

        if (child != null)
            ItemManager.instance.Activate(transform, type);
    }

    public bool IsSameColor(Color color)
    {
       return  materialColor == color;
    }
}
