using System;

public class MaxSpawnedEnemiesRule : ISettable, IDisposable
{
	public event Action IsDone;

	private const int AliveEnemiesCountToDefeat = 10;

	private IReadOnlyVariable<int> _aliveEnemyCount;

	private GameRules _rule;

	public MaxSpawnedEnemiesRule(IReadOnlyVariable<int> aliveEnemyCount)
	{
		_rule = GameRules.HeroIsDead;

		_aliveEnemyCount = aliveEnemyCount;
		_aliveEnemyCount.ValueChanged += OnValueChanged;
	}

	public GameRules Type => _rule;

	private void OnValueChanged(int newCount)
	{
		if (_aliveEnemyCount.Value >= AliveEnemiesCountToDefeat)
			IsDone?.Invoke();
	}

	public void Dispose()
	{
		_aliveEnemyCount.ValueChanged -= OnValueChanged;
	}

	public void UpdateLogic(float deltaTime)
	{

	}
}
