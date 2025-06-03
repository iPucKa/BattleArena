using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameMode
{
	public event Action Win;
	public event Action Defeat;

	private const float TimeToWin = 15;
	private const int KilledEnemiesCountToWin = 10;
	private const int AliveEnemiesCountToDefeat = 10;

	private LevelConfig _levelConfig;
	private Character _character;
	private EnemySpawner _enemySpawner;

	private RuleService _ruleWinService;
	private RuleService _ruleDefeatService;

	private bool _isRunning;

	private ItemList<Enemy> _spawnedEnemies = new();
	private ItemList<Bullet> _bullets = new();

	private List<GameSettings> _winSettings = new();
	private List<GameSettings> _defeatSettings = new();

	private float _time;
	private int _killedCount;

	private bool _mainCharacterKilled;

	private float _currentTimeToWin;

	public GameMode(
		LevelConfig levelConfig,
		Character character,
		EnemySpawner enemySpawner)
	{
		_levelConfig = levelConfig;
		_character = character;
		_enemySpawner = enemySpawner;

		_winSettings = _levelConfig.WinSettings;
		_defeatSettings = _levelConfig.DefeatSettings;
		
		_ruleWinService = new RuleService();
		_ruleDefeatService = new RuleService();

		foreach (GameSettings settings in _winSettings)
		{
			switch (settings.Rule)
			{
				case GameRules.EnemiesKilledCount:
					_ruleWinService.AddTo(settings.Rule, () => _killedCount >= KilledEnemiesCountToWin);
					break;

				case GameRules.MaxLifeTime:
					_ruleWinService.AddTo(settings.Rule, () => _currentTimeToWin >= TimeToWin && _mainCharacterKilled == false);
					break;

				default:
					throw new ArgumentOutOfRangeException($"Unknown rule condition");
			}			
		}		

		foreach (GameSettings settings in _defeatSettings)
		{
			switch (settings.Rule)
			{
				case GameRules.MaxSpawnedEnemies:
					_ruleDefeatService.AddTo(settings.Rule, () => _spawnedEnemies.Count >= AliveEnemiesCountToDefeat || _mainCharacterKilled);
					break;

				case GameRules.HeroIsDead:
					_ruleDefeatService.AddTo(settings.Rule, () => _mainCharacterKilled);
					break;

				default:
					throw new ArgumentOutOfRangeException($"Unknown rule condition");
			}
		}

		_ruleWinService.IsDone += OnWinConditionCompleted;
		_ruleDefeatService.IsDone += OnDefeatConditionCompleted;

		_character.Killed += OnMainHeroKilled;
		_character.Destroyed += OnMainHeroDestroyed;
	}	

	private void OnDefeatConditionCompleted()
	{
		ProcessDefeat();
	}

	private void OnWinConditionCompleted()
	{
		ProcessWin();
	}

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

		ProcessCountingWinTime(deltaTime);

		_ruleWinService.Update(Time.deltaTime);
		_ruleDefeatService.Update(Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_character.Shoot(out Bullet bullet);

			_bullets.Add(bullet);
		}
	}

	private void OnMainHeroDestroyed(MonoDestroyable destroyable)
	{
		_mainCharacterKilled = false;
	}

	private void OnMainHeroKilled()
	{
		_mainCharacterKilled = true;
	}

	private void OnEnemyDestroyed(MonoDestroyable enemy)
	{
		_killedCount++;
		_spawnedEnemies.Remove(enemy as Enemy);
	}

	private void ProcessCountingWinTime(float deltaTime)
	{
		_currentTimeToWin += deltaTime;
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

	private void ProcessEndGame()
	{
		_isRunning = false;

		_ruleWinService.IsDone -= OnWinConditionCompleted;
		_ruleDefeatService.IsDone -= OnDefeatConditionCompleted;

		for (int i = 0; i < _spawnedEnemies.Count; i++)
		{
			_spawnedEnemies.GetBy(i).Destroyed -= OnEnemyDestroyed;
			_spawnedEnemies.GetBy(i).Destroy();
		}

		_spawnedEnemies.Clear();


		for (int i = 0; i < _bullets.Count; i++)
			_bullets.GetBy(i).Destroy();

		_bullets.Clear();

		_character.Killed -= OnMainHeroKilled;
		_character.Destroyed -= OnMainHeroDestroyed;
		_character.Destroy();
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
}
