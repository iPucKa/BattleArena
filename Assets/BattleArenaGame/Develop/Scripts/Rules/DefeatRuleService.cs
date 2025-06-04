using System;

public class DefeatRuleService
{
	public event Action IsCompleted;

	private LevelConfig _levelConfig;
	private Character _character;
	private GameMode _gameMode;

	private ISettable _currentRule;

	private IReadOnlyVariable<int> _aliveEnemyCount;

	public DefeatRuleService(
		LevelConfig levelConfig,
		Character character,
		GameMode gameMode)
	{
		_levelConfig = levelConfig;
		_character = character;
		_gameMode = gameMode;

		_aliveEnemyCount = _gameMode.SpawnedEnemyCount;

		SetCurrrentRule(_levelConfig.DefeatRule.Rule);

		_currentRule.IsDone += OnRuleCompleted;
	}

	public ISettable DefeatRule => _currentRule;

	public void Update(float deltaTime)
	{
		_currentRule.UpdateLogic(deltaTime);
	}

	private void SetCurrrentRule(GameRules rule)
	{
		switch (rule)
		{
			case GameRules.MaxSpawnedEnemies:
				_currentRule = new MaxSpawnedEnemiesRule(_aliveEnemyCount);
				break;

			case GameRules.HeroIsDead:
				_currentRule = new MainHeroDeadRule(_character);
				break;

			default:
				throw new ArgumentOutOfRangeException($"Unknown rule condition");
		}
	}

	private void OnRuleCompleted()
	{
		IsCompleted?.Invoke();
	}
}
