#nullable enable
using System;
using System.Collections.Generic;
using NuclearBand;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; } = null!;

    [SerializeField]
    private EnemySpawnController _enemySpawnController = null!;
    
    [SerializeField]
    private BulletSpawnController _bulletSpawnController = null!;
    
    [SerializeField]
    private PlayerMovementController _playerMovementController = null!;

    [SerializeField] 
    private ShootController _shootController = null!;

    [SerializeField] 
    private GameObject _game = null!;

    private GameWindow? _gameWindow;

    private List<IEnemy> _enemies = new();
    
    private void Awake()
    {
        Instance = this;
        
        _enemySpawnController.Init();
        _bulletSpawnController.Init();
        _game.SetActive(false);
        
        WindowsManager.Init(new WindowsManagerSettings
        {
            InputBlockPath = "GUI/InputBlocker",
            RootPath = "GUI/Canvas"
        });
        var backButtonEventManager = new GameObject().AddComponent<BackButtonEventManager>();
        backButtonEventManager.OnBackButtonPressed += WindowsManager.ProcessBackButton;

        MenuWindow.CreateWindow().Show();
    }

    public void StartGame()
    {
        _gameWindow = GameWindow.CreateWindow(_shootController.Shoot);
        _gameWindow.Show();
        
        _playerMovementController.Init(_gameWindow.MovementJoystick);
        _shootController.Init(_gameWindow.ShootJoystick);
        

        for (var i = 0; i < 1; i++)
        {
            _enemies.Add(_enemySpawnController.SpawnEnemy(
                new Vector2(Random.Range(-1000, 1000), 
                    Random.Range(-500, 500)), new EnemySettings()));
        }
        
        _game.SetActive(true);
    }

    public void FinishGame()
    {
        if (_gameWindow == null)
            throw new NullReferenceException("if (_gameWindow == null || _movementWindow == null)");
        
        _game.SetActive(false);
        _gameWindow.Close();
        MenuWindow.CreateWindow().Show();
    }
}
