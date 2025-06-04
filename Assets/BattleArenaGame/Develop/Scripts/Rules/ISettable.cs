using System;

public interface ISettable
{
	event Action IsDone;

	GameRules Type { get; }

	void UpdateLogic(float deltaTime);
}
