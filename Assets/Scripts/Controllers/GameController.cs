using System.ComponentModel;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GlobalData _globalData;
    public AudioSource audioSource;
    public AudioSource realSource;
    [Header("Blocks")]
    public Block BlockPrefab;

    [Header("Navigation")]
    public MovePoint[] movePoints;

    Controllers _controllers;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        _controllers = new Controllers();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player player = CreatePlayer();

        var blockFactory = new BlockFactory<Block>(_globalData.BlockData);
        var blockPool = new Pool<Block>(blockFactory);

        var compostieMover = new CompositeMoveController();
        var blockSpawner = new BlockSpawnerController(blockPool, _globalData.BlockData);
        compostieMover.Add(player);
        var noteProvider = new NoteRandomizing();
        var secToReach = _globalData.BlockData.timeToReachPlayer;
        var songController = new SongController(audioSource, realSource, noteProvider, secToReach);
        var spawnPositionCalculator = new BlockSpawnPostionCalculator(_globalData.BlockData, movePoints);

        _controllers.Add(songController);
        _controllers.Add(compostieMover);

        var destinationSetter = new PlayerDestinationSelector(player, movePoints);

        _controllers.Initialize();

        var scoreController = new ScoreController(_globalData.ScoreData);
        var trigger = new TriggerActivationController(player, _globalData.PlayerData.TriggerTimeMilliseconds);
        var collectionController = new NoteCollectController(_globalData.BlockData, blockPool);
    }

    private Player CreatePlayer()
    {
        var playerFactory = new PlayerFactory<Player>(_globalData.PlayerData);
        var player = playerFactory.Create();
        return player;
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.deltaTime;
        _controllers.FrameUpdate(deltaTime);
    }

    void FixedUpdate()
    {
        var deltaTime = Time.deltaTime;
        _controllers.FixedFrameUpdate(deltaTime);
    }


}
