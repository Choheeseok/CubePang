using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private const float BOMB_SCALE_X = 0.8f;
    private const float BOMB_SCALE_Y = 100.0f;
    private const float BOMB_SCALE_Z = 0.8f;
    private const float BOMB_OFFSET_Y = 0.5f;
    private const int COLOR_BOMB_CHANCE = 33;
    private const int LINE_BOMB_CHANCE = 66;
    private const int MAX_RANDOM_CHANCE = 99;

    public static ItemManager instance { get; private set; }

    [SerializeField] private GameObject sameColorBombPrefab;
    [SerializeField] private GameObject sameLineBombPrefab;
    [SerializeField] private GameObject sameSideBombPrefab;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void GenerateItem()
    {
        var tileList = GameManager.instance.TileList;
        var emptyTileIndex = FindEmptyTileIndex(tileList);
        var bombType = DetermineBombType();
        var bomb = CreateBomb(tileList[emptyTileIndex], bombType);
        SetupBomb(bomb, tileList[emptyTileIndex]);
    }

    private int FindEmptyTileIndex(List<Tile> tileList)
    {
        int index;
        do
        {
            index = Random.Range(0, tileList.Count);
        } while (tileList[index].child != null);
        return index;
    }

    private (GameObject prefab, CustomVariables.TILE type) DetermineBombType()
    {
        int randomValue = Random.Range(1, 100);
        return randomValue switch
        {
            <= COLOR_BOMB_CHANCE => (sameColorBombPrefab, CustomVariables.TILE.COLOR_BOMB),
            <= LINE_BOMB_CHANCE => (sameLineBombPrefab, CustomVariables.TILE.LINE_BOMB),
            <= MAX_RANDOM_CHANCE => (sameSideBombPrefab, CustomVariables.TILE.SIDE_BOMB),
            _ => throw new System.Exception("잘못된 랜덤 값입니다.")
        };
    }

    private GameObject CreateBomb(Tile targetTile, (GameObject prefab, CustomVariables.TILE type) bombInfo)
    {
        var bomb = Instantiate(bombInfo.prefab, targetTile.transform);
        targetTile.SetChild(bomb, bombInfo.type);
        return bomb;
    }

    private void SetupBomb(GameObject bomb, Tile targetTile)
    {
        bomb.transform.localScale = new Vector3(BOMB_SCALE_X, BOMB_SCALE_Y, BOMB_SCALE_Z);
        bomb.transform.position += bomb.transform.up * BOMB_OFFSET_Y;
        bomb.GetComponent<MeshRenderer>().material.color = targetTile.transform.GetComponent<MeshRenderer>().material.color;
    }

    public void Activate(Transform target, CustomVariables.TILE type)
    {
        var tile = GameManager.instance.GetTile(target);
        StartCoroutine(BingoManager.instance.BingoEvent(type, tile));
        CleanupBomb(tile);
    }

    private void CleanupBomb(Tile tile)
    {
        tile.type = CustomVariables.TILE.EMPTY;
        Destroy(tile.child);
    }
}
