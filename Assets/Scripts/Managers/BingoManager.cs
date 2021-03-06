using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoManager : MonoBehaviour
{
    public static BingoManager instance = null;  // 싱글톤 인스턴스

    public GameObject itemSpawnerPrefab;

    public List<GameObject> itemSpawnerList;

    public List<Item> itemList;  // 아이템 관리할 리스트
    List<int> centerIndices = new List<int>() { 10, 23, 26, 27, 30, 43 };   // 3 x 3 큐브의 가운데 타일 인덱스입니다.

    public int totalItemSummonCount;
    private float currentItemSummonCount;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        itemSpawnerList = new List<GameObject>();

        // 각 게임객체를 담는 리스트를 초기화 합니다.
        itemList = new List<Item>();
    }

    public bool CheckLineEvent(Tile tile)
    {
        //List<Tile> tileList = GameManager.instance.TileList;

        //List<Tile>[] sameSideTiles;
        //sameSideTiles = new List<Tile>[6];
        //List<Tile>[] sameLineTiles;
        //sameLineTiles = new List<Tile>[36];
        //List<Tile> bingoTiles;

        //for (int i = 0; i < 6; ++i)
        //    sameSideTiles[i] = new List<Tile>();
        //for (int i = 0; i < 36; ++i)
        //    sameLineTiles[i] = new List<Tile>();

        //bingoTiles = new List<Tile>();

        //int cubeCount = GameManager.instance.CubeCount;
        //float radius = GameManager.instance.radius * 2;

        //// 빙고의 조건
        //// - 같은 면에 있다 -> Up Vectr 가 같다
        //for (int i = 0; i < cubeCount * cubeCount * 6; ++i)
        //{
        //    tileList[i].SaveCurTileColor();
        //    Vector3 upVector = tileList[i].transform.up;
        //    if (0.5 < upVector.x)
        //        sameSideTiles[(int)CustomVariables.SIDE.PX].Add(tileList[i]);
        //    if (upVector.x < -0.5)
        //        sameSideTiles[(int)CustomVariables.SIDE.NX].Add(tileList[i]);
        //    if (0.5 < upVector.y)
        //        sameSideTiles[(int)CustomVariables.SIDE.PY].Add(tileList[i]);
        //    if (upVector.y < -0.5)
        //        sameSideTiles[(int)CustomVariables.SIDE.NY].Add(tileList[i]);
        //    if (0.5 < upVector.z)
        //        sameSideTiles[(int)CustomVariables.SIDE.PZ].Add(tileList[i]);
        //    if (upVector.z < -0.5)
        //        sameSideTiles[(int)CustomVariables.SIDE.NZ].Add(tileList[i]);
        //}

        //// - 같은 줄에 있다 -> Up Vectr를 제외한 평면에서 한 좌표가 같다
        //for (int i = -cubeCount / 2; i <= cubeCount / 2; ++i)
        //{
        //    int idx = i + cubeCount / 2;
        //    for (int j = 0; j < 9; ++j)
        //    {
        //        // PX
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.PX][j].transform.position.y))   // 가로
        //            sameLineTiles[(int)CustomVariables.SIDE.PX * 6 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.PX][j]);
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.PX][j].transform.position.z))   // 세로
        //            sameLineTiles[(int)CustomVariables.SIDE.PX * 6 + 3 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.PX][j]);
        //        // NX
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.NX][j].transform.position.y))   // 가로
        //            sameLineTiles[(int)CustomVariables.SIDE.NX * 6 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.NX][j]);
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.NX][j].transform.position.z))   // 세로
        //            sameLineTiles[(int)CustomVariables.SIDE.NX * 6 + 3 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.NX][j]);
        //        // PY
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.PY][j].transform.position.x))   // 가로
        //            sameLineTiles[(int)CustomVariables.SIDE.PY * 6 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.PY][j]);
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.PY][j].transform.position.z))   // 세로
        //            sameLineTiles[(int)CustomVariables.SIDE.PY * 6 + 3 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.PY][j]);
        //        // NY
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.NY][j].transform.position.x))   // 가로
        //            sameLineTiles[(int)CustomVariables.SIDE.NY * 6 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.NY][j]);
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.NY][j].transform.position.z))   // 세로
        //            sameLineTiles[(int)CustomVariables.SIDE.NY * 6 + 3 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.NY][j]);
        //        // PZ
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.PZ][j].transform.position.y))   // 가로
        //            sameLineTiles[(int)CustomVariables.SIDE.PZ * 6 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.PZ][j]);
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.PZ][j].transform.position.x))   // 세로
        //            sameLineTiles[(int)CustomVariables.SIDE.PZ * 6 + 3 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.PZ][j]);
        //        // NZ
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.NZ][j].transform.position.y))   // 가로
        //            sameLineTiles[(int)CustomVariables.SIDE.NZ * 6 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.NZ][j]);
        //        if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[(int)CustomVariables.SIDE.NZ][j].transform.position.x))   // 세로
        //            sameLineTiles[(int)CustomVariables.SIDE.NZ * 6 + 3 + idx].Add(sameSideTiles[(int)CustomVariables.SIDE.NZ][j]);
        //    }
        //}

        //for (int i = 0; i < 36; ++i)
        //{
        //    Color[] color = new Color[3];
        //    color[0] = sameLineTiles[i][0].GetComponent<MeshRenderer>().material.color;
        //    color[1] = sameLineTiles[i][1].GetComponent<MeshRenderer>().material.color;
        //    color[2] = sameLineTiles[i][2].GetComponent<MeshRenderer>().material.color;
        //    if (color[0] == color[1] && color[1] == color[2])
        //    {
        //        // Bingo!
        //        bingoTiles.Add(sameLineTiles[i][0]);
        //        bingoTiles.Add(sameLineTiles[i][1]);
        //        bingoTiles.Add(sameLineTiles[i][2]);
        //    }
        //}

        //if (bingoTiles.Count == 0)
        //    return false;

        //for (int i = 0; i < bingoTiles.Count; ++i)
        //{
        //    bingoTiles[i].TakeDamage();
        //}

        //return true;






        List<Tile> tileList = GameManager.instance.TileList;

        List<Tile> sameSideTiles;
        sameSideTiles = new List<Tile>();
        List<Tile>[] sameLineTiles;
        sameLineTiles = new List<Tile>[6];
        List<Tile> bingoTiles;

        for (int i = 0; i < 6; ++i)
            sameLineTiles[i] = new List<Tile>();

        bingoTiles = new List<Tile>();

        int cubeCount = GameManager.instance.CubeCount;
        float radius = GameManager.instance.radius * 2;

        Vector3 upVector = tile.transform.up;
        // 빙고의 조건
        // - 같은 면에 있다 -> Up Vectr 가 같다
        for (int i = 0; i < cubeCount * cubeCount * 6; ++i)
        {
           // tileList[i].SaveCurTileColor();
            if (CustomVariables.IsSimillerVectorDir(upVector, tileList[i].transform.up))
                sameSideTiles.Add(tileList[i]);
        }
        // - 같은 줄에 있다 -> Up Vectr를 제외한 평면에서 한 좌표가 같다
        for (int i = -cubeCount / 2; i <= cubeCount / 2; ++i)
        {
            int idx = i + cubeCount / 2;
            for (int j = 0; j < sameSideTiles.Count; ++j)
            {
                
                if (0.5f< upVector.x ||  upVector.x < -0.5f)
                {
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.y))   // 가로
                        sameLineTiles[idx].Add(sameSideTiles[j]);
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.z))   // 세로
                        sameLineTiles[3 + idx].Add(sameSideTiles[j]);
                }
                if (0.5f < upVector.y || upVector.y < -0.5f)
                {
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.x))   // 가로
                        sameLineTiles[idx].Add(sameSideTiles[j]);
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.z))   // 세로
                        sameLineTiles[3 + idx].Add(sameSideTiles[j]);
                }
                if (0.5f < upVector.z || upVector.z < -0.5f)
                {
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.y))   // 가로
                        sameLineTiles[idx].Add(sameSideTiles[j]);
                    if (CustomVariables.IsSimillerValue(radius * i, sameSideTiles[j].transform.position.x))   // 세로
                        sameLineTiles[3 + idx].Add(sameSideTiles[j]);
                }

            }
        }

        for (int i = 0; i < 6; ++i)
        {
            if (sameLineTiles[i].Count != GameManager.instance.CubeCount)
                continue;

            bool bingo = true;
            Color compareColor = sameLineTiles[i][0].GetComponent<MeshRenderer>().material.color;
            for(int j = 0; j < GameManager.instance.CubeCount; ++j)
            {
                if (false == sameLineTiles[i][j].IsSameColor(compareColor))
                {
                    bingo = false;
                    break;
                }
            }

            if( true == bingo)
            {
                foreach (Tile t in sameLineTiles[i] )
                    bingoTiles.Add(t);
            }
        }

        if (bingoTiles.Count == 0)
            return false;

        for (int i = 0; i < bingoTiles.Count; ++i)
        {
            bingoTiles[i].TakeDamage();
        }

        return true;
    }

    public bool CheckSameColorBombEvent(Tile target, CustomVariables.SIDE side)
    {
        List<Tile> tileList = GameManager.instance.TileList;
        List<Tile> bingoTiles = new List<Tile>();

        foreach(Tile tile in tileList)
        {
            if (
    tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
                bingoTiles.Add(tile);

            //if (side == CustomVariables.SIDE.PX)
            //{
            //    if (0.5f < tile.transform.up.x &&
            //        tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
            //        bingoTiles.Add(tile);
            //}
            //else if (side == CustomVariables.SIDE.NX)
            //{
            //    if (tile.transform.up.x < -0.5f &&
            //         tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
            //        bingoTiles.Add(tile);
            //}
            //else if (side == CustomVariables.SIDE.PY)
            //{
            //    if (0.5f < tile.transform.up.y &&
            //         tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
            //        bingoTiles.Add(tile);
            //}
            //else if (side == CustomVariables.SIDE.NY)
            //{
            //    if (tile.transform.up.y < -0.5f &&
            //         tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
            //        bingoTiles.Add(tile);
            //}
            //else if (side == CustomVariables.SIDE.PZ)
            //{
            //    if (0.5f < tile.transform.up.z &&
            //         tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
            //        bingoTiles.Add(tile);
            //}
            //else if (side == CustomVariables.SIDE.NZ)
            //{
            //    if (tile.transform.up.z < -0.5f &&
            //         tile.IsSameColor(target.GetComponent<MeshRenderer>().material.color))
            //        bingoTiles.Add(tile);
            //}
        }

        if (bingoTiles.Count == 0)
            return false;

        Debug.Log("SameColorBomb : " + side + " - " + bingoTiles.Count);
        foreach (Tile tile in bingoTiles)
        {
            tile.TakeDamage();
        }

        return true;
    }

    public bool CheckSameLineBombEvent(Tile target, int dir)
    {
        List<Tile> tileList = GameManager.instance.TileList;
        List<Tile> bingoTiles = new List<Tile>();

        Vector3 upVector = target.transform.up;
        if (0.5f < upVector.x || upVector.x < -0.5f)
        {
            foreach (Tile tile in tileList)
            {
                if (0 == dir)
                {
                    if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                        CustomVariables.IsSimillerValue(tile.transform.position.y, target.transform.position.y))
                        bingoTiles.Add(tile);
                }
                else if (1 == dir)
                {
                    if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                        CustomVariables.IsSimillerValue(tile.transform.position.z, target.transform.position.z))
                        bingoTiles.Add(tile);
                }
            }
        }
        if (0.5f < upVector.y || upVector.y < -0.5f)
        {
            foreach (Tile tile in tileList)
            {
                if (0 == dir)
                {
                    if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                        CustomVariables.IsSimillerValue(tile.transform.position.x, target.transform.position.x))
                        bingoTiles.Add(tile);
                }
                else if (1 == dir)
                {
                    if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                        CustomVariables.IsSimillerValue(tile.transform.position.z, target.transform.position.z))
                        bingoTiles.Add(tile);
                }
            }
        }
        if (0.5f < upVector.z || upVector.z < -0.5f)
        {
            foreach (Tile tile in tileList)
            {
                if (0 == dir)
                {
                    if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                        CustomVariables.IsSimillerValue(tile.transform.position.y, target.transform.position.y))
                        bingoTiles.Add(tile);
                }
                else if (1 == dir)
                {
                    if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up) &&
                        CustomVariables.IsSimillerValue(tile.transform.position.x, target.transform.position.x))
                        bingoTiles.Add(tile);
                }
            }
        }

        if (bingoTiles.Count == 0)
            return false;

        foreach (Tile tile in bingoTiles)
        {
            tile.TakeDamage();
        }

        return true;
    }
    
    public bool CheckSameSideBombEvent(Tile target)
    {
        List<Tile> tileList = GameManager.instance.TileList;
        List<Tile> bingoTiles = new List<Tile>();

        Vector3 upVector = target.transform.up;
        foreach(Tile tile in tileList)
        {
            if (CustomVariables.IsSimillerVectorDir(upVector, tile.transform.up))
                bingoTiles.Add(tile);
        }

        if (bingoTiles.Count == 0)
            return false;

        foreach (Tile tile in bingoTiles)
        {
            tile.TakeDamage();
        }

        return true;
    }

    public IEnumerator BingoEvent(CustomVariables.TILE type, Tile tile = null)
    {
        GameManager.instance.IsDoingBingo = true;
        yield return new WaitForFixedUpdate();
        if (type == CustomVariables.TILE.EMPTY)
        {
            for (int i = 0; i < centerIndices.Count; ++i)
            {
                while (true == CheckLineEvent(GameManager.instance.TileList[centerIndices[i]]))
                    yield return new WaitForSeconds(0.4f);
            }
        }
        else if (type == CustomVariables.TILE.COLOR_BOMB)
        {
            for (CustomVariables.SIDE i = CustomVariables.SIDE.PX; i < CustomVariables.SIDE.NZ + 1; ++i)
            {
                CheckSameColorBombEvent(tile, i);
                yield return new WaitForSeconds(0.4f);
            }
        }
        else if (type == CustomVariables.TILE.LINE_BOMB)
        {
            for (int i = 0; i < 2; ++i)
            {
                CheckSameLineBombEvent(tile, i);
                yield return new WaitForSeconds(0.4f);
            }
        }
        else if (type == CustomVariables.TILE.SIDE_BOMB)
        {
            CheckSameSideBombEvent(tile);
            yield return new WaitForSeconds(0.4f);
        }
        GameManager.instance.IsDoingBingo = false;
    }

    public Tile GetMiddleTile()
    {
        if (GameManager.instance.CubeCount != 3)
            return null;

        centerIndices.Shuffle();

        for (int i = 0; i < centerIndices.Count; ++i)
        {
            if (GameManager.instance.TileList[centerIndices[i]].type == CustomVariables.TILE.EMPTY)
                return GameManager.instance.TileList[centerIndices[i]];
        }

        return null;
    }
}
