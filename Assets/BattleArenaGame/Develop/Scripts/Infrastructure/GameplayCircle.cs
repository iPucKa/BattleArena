using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayCircle : IDisposable
{
	private MainHeroFactory _mainHeroFactory;

	private MainHeroConfig _mainHeroConfig;
	private Character _character;

	private LevelConfig _levelConfig;
	private WinRuleService _winRuleService;
	private DefeatRuleService _defeatRuleService;

	private ConfirmPopup _confirmPopup;

	private EnemySpawner _enemySpawner;

	private GameMode _gameMode;
	private MonoBehaviour _context;

	public GameplayCircle(
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

		_gameMode = new GameMode(_levelConfig, _character, _enemySpawner);
		_winRuleService = new WinRuleService(_levelConfig, _character, _gameMode);
		_defeatRuleService = new DefeatRuleService(_levelConfig, _character, _gameMode);

		_gameMode.SetRule(_winRuleService, _defeatRuleService);

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

			//_gameMode?.Dispose();
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
}
