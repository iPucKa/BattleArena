using System;

public class MaxLifeTimeRule : ICondition, IDisposable
{
	public event Action IsDone;

	private Character _character;
	private const float TimeToWin = 15;

	private float _currentTime;
	private bool _isCharacterDead;

	public MaxLifeTimeRule(Character character)
	{
		_character = character;

		_character.Killed += OnCharacterDead;
	}

	public void Dispose()
	{
		_character.Killed -= OnCharacterDead;
	}

	public void UpdateLogic(float deltaTime)
	{
		CheckConditions(deltaTime);		
	}

	private void OnCharacterDead()
	{
		_isCharacterDead = true;
	}

	private void CheckConditions(float deltaTime)
	{
		_currentTime += deltaTime;

		if (_currentTime >= TimeToWin && _isCharacterDead == false)
			IsDone?.Invoke();
	}	
}
