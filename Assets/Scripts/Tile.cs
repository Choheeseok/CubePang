using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
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
        transform.GetComponent<MeshRenderer>().material.color = TileColors.RandomColor(GameManager.instance.Level);
        playParticle.GetComponent<ParticleSystem>().Play();
        ++GameManager.instance.Score;

        if (child != null)
            ItemManager.instance.Activate(transform, type);
    }

    public bool IsSameColor(Color color)
    {
       return  transform.GetComponent<MeshRenderer>().material.color == color;
    }
}
