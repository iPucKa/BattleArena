using System;

public class MaxSpawnedEnemiesRule : ICondition, IDisposable
{
	public event Action IsDone;

	private const int AliveEnemiesCountToDefeat = 10;

	private IReadOnlyVariable<int> _currentCount;

	public MaxSpawnedEnemiesRule(IReadOnlyVariable<int> currentCount)
	{
		_currentCount = currentCount;
		_currentCount.ValueChanged += OnValueChanged;
	}

	private void OnValueChanged(int value)
	{
		if (_currentCount.Value >= AliveEnemiesCountToDefeat)
			IsDone?.Invoke();
	}

	public void Dispose()
	{
		_currentCount.ValueChanged -= OnValueChanged;
	}

	public void UpdateLogic(float deltaTime)
	{

	}
}
