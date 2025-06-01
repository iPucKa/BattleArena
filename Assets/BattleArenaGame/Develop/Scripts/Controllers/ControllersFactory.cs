using UnityEngine;

public class ControllersFactory
{
	public PlayerDirectionalMovableController CreatePlayerDirectionalMovableController(IDirectionalMovable movable)
	{
		return new PlayerDirectionalMovableController(movable);
	}

	public AlongMovableVelocityRotatableController CreateAlongMovableVelocityRotatableController(
		IDirectionalMovable movable,
		IDirectionalRotatable rotatable)
	{
		return new AlongMovableVelocityRotatableController(rotatable, movable);
	}

	public CompositeController CreateMainHeroPlayerController(Character character)
	{
		return new CompositeController(
			CreatePlayerDirectionalMovableController(character),
			CreateAlongMovableVelocityRotatableController(character, character));
	}

	public EnemyAgroController CreateEnemyAgroController(
		Enemy character,
		Transform target,
		float agroRange,
		float minDistanceToTarget,
		float timeForIdle)
	{
		return new EnemyAgroController(character, target, agroRange, minDistanceToTarget, timeForIdle);
	}

	public EnemyIdleController CreateEnemyIdleController(Enemy enemy)
	{
		Debug.Log("Установили контроллер");

		return new EnemyIdleController(enemy);
	}
}
