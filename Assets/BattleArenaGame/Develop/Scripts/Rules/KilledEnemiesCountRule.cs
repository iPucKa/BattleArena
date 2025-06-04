using System;

public class KilledEnemiesCountRule : ISettable, IDisposable
{
	public event Action IsDone;
	
	private const int KilledEnemiesCountToWin = 10;

	private IReadOnlyVariable<int> _killedEnemyCount;

	private GameRules _rule;

	public KilledEnemiesCountRule(IReadOnlyVariable<int> killedEnemyCount)
	{
		_rule = GameRules.HeroIsDead;

		_killedEnemyCount = killedEnemyCount;	
		_killedEnemyCount.ValueChanged += OnValueChanged;
	}	

	public GameRules Type => _rule;

	private void OnValueChanged(int newCount)
	{
		if(_killedEnemyCount.Value >= KilledEnemiesCountToWin)
			IsDone?.Invoke();
	}

	public void Dispose()
	{
		_killedEnemyCount.ValueChanged -= OnValueChanged;
	}

	public void UpdateLogic(float deltaTime)
	{
		
	}
}
