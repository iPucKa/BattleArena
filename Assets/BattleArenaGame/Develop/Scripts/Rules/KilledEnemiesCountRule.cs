using System;

public class KilledEnemiesCountRule : ICondition, IDisposable
{
	public event Action IsDone;
	
	private const int KilledEnemiesCountToWin = 10;

	private IReadOnlyVariable<int> _currentCount;
	private IReadOnlyVariable<int> _maxCount;

	public KilledEnemiesCountRule(IReadOnlyVariable<int> currentCount, IReadOnlyVariable<int> maxCount)
	{
		_currentCount = currentCount;
		_maxCount = maxCount;

		_currentCount.ValueChanged += OnValueChanged;
		_maxCount.ValueChanged += OnValueChanged;
	}	

	//public GameRules Type => _rule;

	private void OnValueChanged(int newCount)
	{
		if(_maxCount.Value -_currentCount.Value >= KilledEnemiesCountToWin)
			IsDone?.Invoke();
	}

	public void Dispose()
	{
		_currentCount.ValueChanged -= OnValueChanged;
		_maxCount.ValueChanged -= OnValueChanged;
	}

	public void UpdateLogic(float deltaTime)
	{
		
	}
}
