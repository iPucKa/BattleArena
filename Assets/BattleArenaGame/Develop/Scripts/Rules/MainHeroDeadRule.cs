using System;

public class MainHeroDeadRule : ISettable, IDisposable
{
	public event Action IsDone;

    private Character _character;
	private GameRules _rule;

	public GameRules Type => _rule;

	public MainHeroDeadRule(Character character)
	{
		_rule = GameRules.HeroIsDead;

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
