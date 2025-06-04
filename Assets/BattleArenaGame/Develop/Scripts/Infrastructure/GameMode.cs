using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameMode
{
	public event Action Win;
	public event Action Defeat;

	private LevelConfig _levelConfig;
	private Character _character;
	private EnemySpawner _enemySpawner;

	private WinRuleService _ruleWinService;
	private DefeatRuleService _ruleDefeatService;

	private ItemList<Enemy> _spawnedEnemies = new();
	private ItemList<Bullet> _bullets = new();

	private ReactiveVariable<int> _aliveEnemyCount;
	private ReactiveVariable<int> _killedEnemyCount;

	private bool _isRunning;
	private float _time;
	private int _killedCount;	

	public GameMode(
		LevelConfig levelConfig,
		Character character,
		EnemySpawner enemySpawner)
	{
		_levelConfig = levelConfig;
		_character = character;
		_enemySpawner = enemySpawner;

		_aliveEnemyCount = new ReactiveVariable<int>();
		_killedEnemyCount = new ReactiveVariable<int>();		
	}

	public IReadOnlyVariable<int> SpawnedEnemyCount => _aliveEnemyCount;

	public IReadOnlyVariable<int> KilledEnemyCount => _killedEnemyCount;	

	public void Start()
	{
		GenerateEnemy(_levelConfig.CooldownSpawnTime);

		_isRunning = true;
	}

	public void Update(float deltaTime)
	{
		if (_isRunning == false)
			return;

		GenerateEnemy(deltaTime);		

		_ruleWinService.Update(Time.deltaTime);
		_ruleDefeatService.Update(Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_character.Shoot(out Bullet bullet);

			_bullets.Add(bullet);
		}
	}

	public void SetRule(WinRuleService winService, DefeatRuleService defeatService)
	{
		_ruleWinService = winService;
		_ruleDefeatService = defeatService;

		_ruleWinService.IsCompleted += OnWinConditionCompleted;
		_ruleDefeatService.IsCompleted += OnDefeatConditionCompleted;
	}

	private void OnDefeatConditionCompleted()
	{
		ProcessDefeat();
	}

	private void OnWinConditionCompleted()
	{
		ProcessWin();
	}

	private void OnEnemyDestroyed(MonoDestroyable enemy)
	{
		_killedCount++;
		_spawnedEnemies.Remove(enemy as Enemy);
		
		_aliveEnemyCount.Value = _spawnedEnemies.Count;
		_killedEnemyCount.Value = _killedCount;
	}

	private void GenerateEnemy(float deltaTime)
	{
		_time += deltaTime;

		if (_time >= _levelConfig.CooldownSpawnTime)
		{
			for (int i = 0; i < _levelConfig.EnemiesCount; i++)
			{
				Enemy enemy = _enemySpawner.Spawn(_levelConfig.EnemyConfig, GetRandomSpawnPoint());
				_spawnedEnemies.Add(enemy);
				_aliveEnemyCount.Value = _spawnedEnemies.Count;

				enemy.Destroyed += OnEnemyDestroyed;
			}

			_time = 0;
		}
	}

	private Vector3 GetRandomSpawnPoint()
	{
		int index = Random.Range(0, _levelConfig.EnemiesSpawnPoints.Count);
		Vector3 spawnPoint = _levelConfig.EnemiesSpawnPoints[index];
		return spawnPoint;
	}

	private void ProcessDefeat()
	{
		ProcessEndGame();
		Defeat?.Invoke();
	}

	private void ProcessWin()
	{
		ProcessEndGame();
		Win?.Invoke();
	}

	private void ProcessEndGame()
	{
		_isRunning = false;

		_ruleWinService.IsCompleted -= OnWinConditionCompleted;
		_ruleDefeatService.IsCompleted -= OnDefeatConditionCompleted;

		for (int i = 0; i < _spawnedEnemies.Count; i++)
		{
			_spawnedEnemies.GetBy(i).Destroyed -= OnEnemyDestroyed;
			_spawnedEnemies.GetBy(i).Destroy();
		}

		_spawnedEnemies.Clear();


		for (int i = 0; i < _bullets.Count; i++)
			_bullets.GetBy(i).Destroy();

		_bullets.Clear();

		_character.Destroy();
	}
}
