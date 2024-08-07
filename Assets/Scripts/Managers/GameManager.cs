﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public AudioSource backgroundMusicSource; // 배경 음악용 오디오 소스
    [SerializeField]
    private UIController uiController;

    private Color originTileColor;

    public bool PlayerTurnEnd { get; set; }
    public int CubeCount { get; set; }           // 큐브의 규격 : 3X3 => 3,  4X4 => 4  
    public float radius;         // 큐브의 한 블럭의 반지름
    public uint Level { get; set; }
    public List<Tile> TileList { get; set; }   // 타일을 관리할 리스트

    public Dictionary<CustomVariables.COLOR,  uint> TileCountDictionary { get; set; } // 타일의 개수를 관리할 리스트
    public List<KeyValuePair<Transform, CustomVariables.TILE>> ItemList { get; set; }
    public bool IsCubeSelected { get; set; }
    public CustomVariables.TURN PlayerTurn { get; set; }
    public int currentTurn { get;set; }

    public uint Score { get; set; }

    public int itemPercentage;

    public bool IsDoingBingo { get; set; }

    private void Awake()
    {
        // 중복된 인스턴스 객체 생성을 방지합니다.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        CubeCount = 3;
        radius = 0.5f;
        Level = 4;
        itemPercentage = 20;

        PlayerTurn = CustomVariables.TURN.PLAYER_TURN;
        TileList = new List<Tile>();
        TileCountDictionary = new Dictionary<CustomVariables.COLOR, uint>()
        {
            {CustomVariables.COLOR.RED, 0},
            {CustomVariables.COLOR.GREEN, 0},
            {CustomVariables.COLOR.BLUE, 0},
            {CustomVariables.COLOR.YELLOW, 0},
            {CustomVariables.COLOR.CYAN, 0},
            {CustomVariables.COLOR.MAGENTA, 0},
        };
        ItemList = new List<KeyValuePair<Transform, CustomVariables.TILE>>();

        currentTurn = 0;
        IsCubeSelected = false;
        PlayerTurnEnd = false;

        IsDoingBingo = false;
    }

    private void Start()
    {
        backgroundMusicSource.Play();
    }

    void Update()
    {
        ProcessInput();
        ProcessTurn();
    }

    private void ProcessInput()
    {
        if (!IsCubeSelected)
            CameraManager.instance.RotateInput();
        CameraManager.instance.ProcessInput();
    }

    private void ProcessTurn()
    {
        if (PlayerTurn == CustomVariables.TURN.PLAYER_TURN)
        {       // 플레이어의 조작
            if (PlayerTurnEnd)
            {
                PlayerTurnEnd = false;
                NextTurn();
            }
            //if (false == IsDoingBingo)
                CubeManager.instance.ProcessMouseInput();
        }
        else if (PlayerTurn == CustomVariables.TURN.EVENT_TURN)
        {       // 아이템 타일 파괴 이벤트
            foreach (var item in ItemList)
                ItemManager.instance.Activate(item.Key, item.Value);
            ItemList.Clear();
            NextTurn();
        }
        else if (PlayerTurn == CustomVariables.TURN.PREPARE_TURN)
        {
            if (200 < Score)
                Level = 6;
            else if (100 < Score)
                Level = 5;

            if (Random.Range(0, 100) < itemPercentage)
                ItemManager.instance.GenerateItem();

            NextTurn();
        }
        else
        {
            Debug.LogError("잘못된 접근입니다.");
        }
        ConfirmBingo();
        uiController.UpdateScore(Score);
        uiController.UpdateColorCount(TileCountDictionary, Level);

    }

    public void NextTurn()
    {
        if (PlayerTurn == CustomVariables.TURN.PLAYER_TURN)
            PlayerTurn = CustomVariables.TURN.EVENT_TURN;
        else if (PlayerTurn == CustomVariables.TURN.EVENT_TURN)
            PlayerTurn = CustomVariables.TURN.PREPARE_TURN;
        else if (PlayerTurn == CustomVariables.TURN.PREPARE_TURN)
            PlayerTurn = CustomVariables.TURN.PLAYER_TURN;
        else
            Debug.LogError("잘못된 턴");
    }

    public void ConfirmBingo()
    {
        StartCoroutine(BingoManager.instance.BingoEvent(CustomVariables.TILE.EMPTY));
    }

    public Tile GetTile(Transform target)
    {
        return TileList.Find(x => x.transform == target);
    }
}
