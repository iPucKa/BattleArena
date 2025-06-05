using System;

public interface ICondition
{
	event Action IsDone;

	void UpdateLogic(float deltaTime);
}
