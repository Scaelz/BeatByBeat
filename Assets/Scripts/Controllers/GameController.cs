using UnityEngine;
using UnityEngine.SceneManagement;
using EventBusSystem;

public class GameController : MonoBehaviour, IApplicationRequestHandler
{
    [SerializeField]
    private GlobalData _globalData;
    private AudioSource _mockSource;
    private AudioSource _realSource;
    private MovePoint[] _movePoints;
    private GameUIController _uiController;
    private SongController _songController;
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
        EventBus.Subscribe(this);
        _movePoints = FindObjectsOfType<MovePoint>();
        _uiController = FindObjectOfType<GameUIController>();
        var secToReach = _globalData.BlockData.timeToReachPlayer;
        if (_uiController != null)
        {
            _uiController.Init(_globalData.UIData.SongPickButtonPrefab,
                               _globalData.MusicData.TrackList);
        }
        _mockSource = GameObject.Instantiate(_globalData.MusicData.MockSource) as AudioSource;
        _realSource = GameObject.Instantiate(_globalData.MusicData.RealSource) as AudioSource;

        Player player = CreatePlayer();
        var compostieMover = new CompositeMoveController();
        compostieMover.Add(player);

        var blockTools = NoteBlocksInitialize<Block>();

        var noteProvider = new NoteRandomizing();
        _songController = new SongController(_mockSource, _realSource, noteProvider, secToReach);
        var spawnPositionCalculator = new BlockSpawnPostionCalculator(_globalData.BlockData, _movePoints);
        var destinationSetter = new PlayerDestinationSelector(player, _movePoints);

        _controllers.Add(_songController);
        _controllers.Add(compostieMover);
        _controllers.Initialize();

        var scoreController = new ScoreController(_globalData.ScoreData);
        var trigger = new TriggerActivationController(player, _globalData.PlayerData.TriggerTimeMilliseconds);
        var collectionController = new NoteCollectController(_globalData.BlockData, blockTools.Pool);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(this);
    }

    private Player CreatePlayer()
    {
        var playerFactory = new PlayerFactory<Player>(_globalData.PlayerData);
        var player = playerFactory.Create();
        return player;
    }

    private (BlockSpawnerController<T> Spawner, Pool<T> Pool) NoteBlocksInitialize<T>() where T : MonoBehaviour, IBlock
    {
        var blockFactory = new BlockFactory<T>(_globalData.BlockData);
        var blockPool = new Pool<T>(blockFactory);
        var blockSpawner = new BlockSpawnerController<T>(blockPool, _globalData.BlockData);
        return (blockSpawner, blockPool);
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

    public void ApplycationRequestHandle(ApplicationRequest request)
    {
        switch (request)
        {
            case ApplicationRequest.Reload:
                _songController.RepeatTrack();
                break;
            case ApplicationRequest.Restart:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            default:
                return;
        }
    }
}
