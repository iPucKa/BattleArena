using System;

public class MainHeroDeadRule : ICondition, IDisposable
{
	public event Action IsDone;

    private Character _character;

	public MainHeroDeadRule(Character character)
	{
		_character = character;
		_character.Killed += OnCharacterDead;
	}

	public void Dispose()
	{
		_character.Killed -= OnCharacterDead;
	}

	private void OnCharacterDead()
	{
		IsDone?.Invoke();
	}

	public void UpdateLogic(float deltaTime)
	{
		
	}
}
