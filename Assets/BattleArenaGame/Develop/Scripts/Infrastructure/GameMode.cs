using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameMode
{
	public event Action Win;
	public event Action Defeat;

	private const float TimeToWin = 60;
	private const int KilledEnemiesCountToWin = 10;
	private const int AliveEnemiesCountToDefeat = 10;

	private LevelConfig _levelConfig;
	private Character _character;
	private EnemySpawner _enemySpawner;

	private bool _isRunning;
	private List<Enemy> _spawnedEnemies = new List<Enemy>();
	private List<Bullet> _bullets = new List<Bullet>();

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

		_character.Killed += OnMainHeroKilled;
		_character.Destroyed += OnMainHeroDestroyed;
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

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_character.Shoot(out Bullet bullet);

			_bullets.Add(bullet);
		}

		GenerateEnemy(deltaTime);

		ProcessCountingWinTime(deltaTime);

		if (DefeatConditionCompleted())
		{
			ProcessDefeat();
			return;
		}

		if (WinConditionCompleted())
		{
			ProcessWin();
			return;
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

	private bool WinConditionCompleted()
	{
		switch (_levelConfig.WinRule)
		{
			case GameWinRules.EnemiesKilledCount:
				return _killedCount >= KilledEnemiesCountToWin;
				
			case GameWinRules.CurrentTimeAlive:
				return _currentTimeToWin >= TimeToWin && _mainCharacterKilled == false;

			default:
				return false;
		}
	}

	private bool DefeatConditionCompleted() 
	{
		switch (_levelConfig.DefeatRule)
		{
			case GameDefeatRules.MaxSpawnedEnemies:
				return _spawnedEnemies.Count >= AliveEnemiesCountToDefeat || _mainCharacterKilled;

			case GameDefeatRules.HeroIsDead:
				return _mainCharacterKilled;

			default:
				return false;
		}
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

		foreach (Enemy enemy in _spawnedEnemies)
		{
			enemy.Destroyed -= OnEnemyDestroyed;
			enemy.Destroy();
		}

		_spawnedEnemies.Clear();		

		foreach(Bullet bullet in _bullets)
			bullet.Destroy();
		
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
