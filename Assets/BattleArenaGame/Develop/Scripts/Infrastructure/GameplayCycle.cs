using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayCycle : IDisposable
{
	private MainHeroFactory _mainHeroFactory;

	private MainHeroConfig _mainHeroConfig;
	private Character _character;

	private LevelConfig _levelConfig;

	private RulesFactory _rulesFactory;

	private ReactiveVariable<int> _currentEnemyCount = new();
	private ReactiveVariable<int> _maxEnemyCount = new();

	private ICondition _winCondition;
	private ICondition _defeatCondition;

	private ConfirmPopup _confirmPopup;

	private EnemySpawner _enemySpawner;

	private GameMode _gameMode;
	private MonoBehaviour _context;

	public GameplayCycle(
		MainHeroFactory mainHeroFactory, 
		MainHeroConfig mainHeroConfig,
		LevelConfig levelConfig,
		ConfirmPopup confirmPopup,
		EnemySpawner enemySpawner, 
		MonoBehaviour context)
	{
		_mainHeroFactory = mainHeroFactory;
		_mainHeroConfig = mainHeroConfig;
		_levelConfig = levelConfig;
		_confirmPopup = confirmPopup;
		_enemySpawner = enemySpawner;
		_context = context;
	}

	public IEnumerator PrepareScene()
	{		
		yield return SceneManager.LoadSceneAsync(_levelConfig.EnvironmentSceneName, LoadSceneMode.Additive);		
	}	

	public IEnumerator Launch()
	{
		_character = _mainHeroFactory.Create(_mainHeroConfig, _levelConfig.MainHeroStartPosition);

		_confirmPopup.Show();
		_confirmPopup.ShowMessage($"Press {KeyCode.F.ToString()} for begin");

		yield return _confirmPopup.WaitConfirm(KeyCode.F);

		_confirmPopup.Hide();

		MyCollection<Enemy> enemies = new MyCollection<Enemy>(_currentEnemyCount, _maxEnemyCount);

		_rulesFactory = new RulesFactory();

		_winCondition = SetCondition(_levelConfig.WinRule);
		_defeatCondition = SetCondition(_levelConfig.DefeatRule);

		_gameMode = new GameMode(_levelConfig, _character, _enemySpawner, _winCondition, _defeatCondition, enemies);		

		_gameMode.Win += OnGameModeWin;
		_gameMode.Defeat += OnGameModeDefeat;

		_gameMode.Start();
	}

	public void Update(float deltaTime) => _gameMode?.Update(deltaTime);

	public void Dispose()
	{
		OnGameModeEnded();
	}

	private void OnGameModeEnded()
	{
		if (_gameMode != null)
		{
			_gameMode.Win -= OnGameModeWin;
			_gameMode.Defeat -= OnGameModeDefeat;
		}
	}

	private void OnGameModeDefeat()
	{
		OnGameModeEnded();
		Debug.Log("Defeat");
		_context.StartCoroutine(Launch());
	}

	private void OnGameModeWin()
	{
		OnGameModeEnded();
		Debug.Log("Win");
		SceneManager.LoadScene("WinScene");
	}

	private ICondition SetCondition(GameRules rule)
	{
		switch (rule)
		{
			case GameRules.EnemiesKilledCount:
				return _rulesFactory.CreateKilledEnemiesCountRule(_currentEnemyCount, _maxEnemyCount);

			case GameRules.MaxLifeTime:
				return _rulesFactory.CreateMaxLifeTimeRule(_character);

			case GameRules.MaxSpawnedEnemies:
				return _rulesFactory.CreateMaxSpawnedEnemiesRule(_currentEnemyCount);

			case GameRules.HeroIsDead:
				return _rulesFactory.CreateMainHeroDeadRule(_character);

			default:
				throw new ArgumentOutOfRangeException($"Unknown rule condition");
		}
	}
}
