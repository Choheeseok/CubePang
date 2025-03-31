using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int DEFAULT_CUBE_COUNT = 3;
    private const float DEFAULT_RADIUS = 0.5f;
    private const uint DEFAULT_LEVEL = 4;
    private const int DEFAULT_ITEM_PERCENTAGE = 20;
    private const uint SCORE_FOR_LEVEL_5 = 100;
    private const uint SCORE_FOR_LEVEL_6 = 200;

    public static GameManager instance { get; private set; }

    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private UIController uiController;

    private Color originTileColor;

    public bool PlayerTurnEnd { get; set; }
    public int CubeCount { get; set; }
    public float radius { get; private set; }
    public uint Level { get; set; }
    public List<Tile> TileList { get; private set; }
    public Dictionary<CustomVariables.COLOR, uint> TileCountDictionary { get; private set; }
    public List<KeyValuePair<Transform, CustomVariables.TILE>> ItemList { get; private set; }
    public bool IsCubeSelected { get; set; }
    public CustomVariables.TURN PlayerTurn { get; set; }
    public int currentTurn { get; set; }
    public uint Score { get; set; }
    public int itemPercentage { get; private set; }
    public bool IsDoingBingo { get; set; }

    private void Awake()
    {
        InitializeSingleton();
        InitializeGameState();
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

    private void InitializeGameState()
    {
        CubeCount = DEFAULT_CUBE_COUNT;
        radius = DEFAULT_RADIUS;
        Level = DEFAULT_LEVEL;
        itemPercentage = DEFAULT_ITEM_PERCENTAGE;

        PlayerTurn = CustomVariables.TURN.PLAYER_TURN;
        TileList = new List<Tile>();
        InitializeTileCountDictionary();
        ItemList = new List<KeyValuePair<Transform, CustomVariables.TILE>>();

        currentTurn = 0;
        IsCubeSelected = false;
        PlayerTurnEnd = false;
        IsDoingBingo = false;
    }

    private void InitializeTileCountDictionary()
    {
        TileCountDictionary = new Dictionary<CustomVariables.COLOR, uint>();
        foreach (CustomVariables.COLOR color in System.Enum.GetValues(typeof(CustomVariables.COLOR)))
        {
            TileCountDictionary[color] = 0;
        }
    }

    private void Start()
    {
        backgroundMusicSource.Play();
    }

    private void Update()
    {
        ProcessInput();
        ProcessTurn();
    }

    private void ProcessInput()
    {
        if (!IsCubeSelected)
        {
            CameraManager.instance.RotateInput();
        }
        CameraManager.instance.ProcessInput();
    }

    private void ProcessTurn()
    {
        switch (PlayerTurn)
        {
            case CustomVariables.TURN.PLAYER_TURN:
                ProcessPlayerTurn();
                break;
            case CustomVariables.TURN.EVENT_TURN:
                ProcessEventTurn();
                break;
            case CustomVariables.TURN.PREPARE_TURN:
                ProcessPrepareTurn();
                break;
            default:
                Debug.LogError("잘못된 턴 상태입니다.");
                break;
        }

        UpdateUI();
    }

    private void ProcessPlayerTurn()
    {
        if (PlayerTurnEnd)
        {
            PlayerTurnEnd = false;
            NextTurn();
        }
        CubeManager.instance.ProcessMouseInput();
    }

    private void ProcessEventTurn()
    {
        foreach (var item in ItemList)
        {
            ItemManager.instance.Activate(item.Key, item.Value);
        }
        ItemList.Clear();
        NextTurn();
    }

    private void ProcessPrepareTurn()
    {
        UpdateLevel();
        TryGenerateItem();
        NextTurn();
    }

    private void UpdateLevel()
    {
        if (Score > SCORE_FOR_LEVEL_6)
        {
            Level = 6;
        }
        else if (Score > SCORE_FOR_LEVEL_5)
        {
            Level = 5;
        }
    }

    private void TryGenerateItem()
    {
        if (Random.Range(0, 100) < itemPercentage)
        {
            ItemManager.instance.GenerateItem();
        }
    }

    private void UpdateUI()
    {
        ConfirmBingo();
        uiController.UpdateScore(Score);
        uiController.UpdateColorCount(TileCountDictionary, Level);
    }

    public void NextTurn()
    {
        PlayerTurn = PlayerTurn switch
        {
            CustomVariables.TURN.PLAYER_TURN => CustomVariables.TURN.EVENT_TURN,
            CustomVariables.TURN.EVENT_TURN => CustomVariables.TURN.PREPARE_TURN,
            CustomVariables.TURN.PREPARE_TURN => CustomVariables.TURN.PLAYER_TURN,
            _ => throw new System.Exception("잘못된 턴 상태입니다.")
        };
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
