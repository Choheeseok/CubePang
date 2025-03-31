using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private CustomVariables.COLOR color;
    private Color materialColor;
    private Color prevTileColor;
    
    public GameObject child;
    public CustomVariables.TILE type;
    public GameObject particlePrefab;
    private GameObject particleInstance;

    private void Awake()
    {
        particleInstance = Instantiate(particlePrefab, transform);
        child = null;
        type = CustomVariables.TILE.EMPTY;
    }

    public void SetChild(GameObject childObject, CustomVariables.TILE tileType)
    {
        child = childObject;
        type = tileType;
    }

    public void SetColor(KeyValuePair<CustomVariables.COLOR, Color> colorPair)
    {
        UpdateTileCount(color, colorPair.Key);
        
        color = colorPair.Key;
        materialColor = colorPair.Value;
        UpdateMaterialColor();
    }
    
    public void SaveCurrentColor()
    {
        prevTileColor = GetComponent<MeshRenderer>().material.color;
    }

    public void RestorePreviousColor()
    {
        GetComponent<MeshRenderer>().material.color = prevTileColor;
    }

    public void TakeDamage()
    {
        var newColor = TileColors.RandomColor(GameManager.instance.Level);
        UpdateTileCount(color, newColor.Key);
        
        color = newColor.Key;
        materialColor = newColor.Value;
        UpdateMaterialColor();
        
        PlayParticleEffect();
        GameManager.instance.Score++;

        if (child != null)
        {
            ItemManager.instance.Activate(transform, type);
        }
    }

    public bool IsSameColor(Color otherColor)
    {
        return materialColor == otherColor;
    }

    private void UpdateTileCount(CustomVariables.COLOR oldColor, CustomVariables.COLOR newColor)
    {
        GameManager.instance.TileCountDictionary[oldColor]--;
        GameManager.instance.TileCountDictionary[newColor]++;
    }

    private void UpdateMaterialColor()
    {
        GetComponent<MeshRenderer>().material.color = materialColor;
    }

    private void PlayParticleEffect()
    {
        particleInstance.GetComponent<ParticleSystem>().Play();
    }
}
