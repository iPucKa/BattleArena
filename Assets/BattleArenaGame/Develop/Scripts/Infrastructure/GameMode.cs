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

	private MyCollection<Enemy> _enemies;
	private MyCollection<Bullet> _bullets = new();

	private ICondition _winCondition;
	private ICondition _defeatCondition;

	private bool _isRunning;
	private float _time;

	public GameMode(
		LevelConfig levelConfig,
		Character character,
		EnemySpawner enemySpawner,
		ICondition winCondition,
		ICondition defeatCondition,
		MyCollection<Enemy> enemies)
	{
		_levelConfig = levelConfig;
		_character = character;
		_enemySpawner = enemySpawner;
		_winCondition = winCondition;
		_defeatCondition = defeatCondition;
		_enemies = enemies;	
	}

	public void Start()
	{
		_winCondition.IsDone += OnWinConditionCompleted;
		_defeatCondition.IsDone += OnDefeatConditionCompleted;

		GenerateEnemy(_levelConfig.CooldownSpawnTime);

		_isRunning = true;
	}

	public void Update(float deltaTime)
	{
		if (_isRunning == false)
			return;

		GenerateEnemy(deltaTime);

		_winCondition.UpdateLogic(Time.deltaTime);
		_defeatCondition.UpdateLogic(Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			_character.Shoot(out Bullet bullet);

			_bullets.Add(bullet);
		}
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
		_enemies.Remove(enemy as Enemy);
	}

	private void GenerateEnemy(float deltaTime)
	{
		_time += deltaTime;

		if (_time >= _levelConfig.CooldownSpawnTime)
		{
			for (int i = 0; i < _levelConfig.EnemiesCount; i++)
			{
				Enemy enemy = _enemySpawner.Spawn(_levelConfig.EnemyConfig, GetRandomSpawnPoint());
				_enemies.Add(enemy);

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

		_winCondition.IsDone -= OnWinConditionCompleted;
		_defeatCondition.IsDone -= OnDefeatConditionCompleted;

		for (int i = 0; i < _enemies.Count; i++)
		{
			_enemies.GetBy(i).Destroyed -= OnEnemyDestroyed;
			_enemies.GetBy(i).Destroy();
		}

		_enemies.Clear();

		for (int i = 0; i < _bullets.Count; i++)
			_bullets.GetBy(i).Destroy();

		_bullets.Clear();

		_character.Destroy();
	}
}
