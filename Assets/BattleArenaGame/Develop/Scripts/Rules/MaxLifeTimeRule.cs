using System;

public class MaxLifeTimeRule : ISettable, IDisposable
{
	public event Action IsDone;

	private Character _character;
	private const float TimeToWin = 15;

	private float _currentTime;
	private bool _isCharacterDead;

	private GameRules _rule;

	public MaxLifeTimeRule(Character character)
	{
		_character = character;

		_character.Killed += OnCharacterDead;
	}	

	public GameRules Type => _rule;

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
