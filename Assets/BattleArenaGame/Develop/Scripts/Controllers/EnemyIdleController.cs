using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleController : Controller
{
	private Enemy _enemy;

	private const int _xRange = 4;
	private const float _zRange = 4f;

	private Vector3 _target;

	private const float MinDistanceToTarget = 0.01f;
	private const float TimeToChangeDirection = 3f;

	private float _time;

	private NavMeshPath _pathToTarget = new NavMeshPath();

	public EnemyIdleController(Enemy enemy)
	{
		_enemy = enemy;

		_target = GetRandomPosition(_enemy.transform);
	}

	protected override void UpdateLogic(float deltaTime)
	{
		_time += Time.deltaTime;

		if (_enemy.InSpawnProcess(out float elapsedTime))
			return;

		//if (_enemy.IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData))
		//{
		//	_enemy.SetRotationDirection(offMeshLinkData.endPos - offMeshLinkData.startPos);
		//	return;
		//}

		_enemy.SetRotationDirection(_enemy.CurrentVelocity);

		if(_enemy.TryGetPath(_target, _pathToTarget))
		{
			float distanceToTarget = NavMeshUtils.GetPathLength(_pathToTarget);

			if(_time >= TimeToChangeDirection)			
				ChangeDirection();			
			
			if (IsTargetReached(distanceToTarget))			
				ChangeDirection();			

			Move();
			return;
		}

		_enemy.StopMove();
	}

	private bool IsTargetReached(float distanceToTarget) => distanceToTarget <= MinDistanceToTarget;

	private Vector3 GetRandomPosition(Transform source)
	{
		float xPosition = source.position.x + Random.Range(-_xRange, _xRange);
		float zPosition = source.position.z + Random.Range(-_zRange, _zRange);

		return new Vector3(xPosition, 0f, zPosition);
	}

	private void ChangeDirection()
	{
		_time = 0;
		_target = GetRandomPosition(_enemy.transform);		
	}

	private void Move()
	{
		_enemy.ResumeMove();
		_enemy.SetDestination(_target);
	}
}
