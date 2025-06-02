using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoDestroyable, IMovable, ICanSpawn, IKillable
{
	public event Action Killed;

	private NavMeshAgent _agent;
	private AgentMover _mover;
	private TransformDirectionalRotator _rotator;

	private Timer _spawnTimer;

	private float _timeToSpawn;

	public Vector3 CurrentVelocity => _mover.CurrentVelocity;

	public float TimeToSpawn => _spawnTimer.TimeLimit;

	public void Initialize(
		NavMeshAgent agent,
		AgentMover mover,
		TransformDirectionalRotator rotator,
		Timer spawnTimer,
		float timeToSpawn)
	{
		_agent = agent;
		_mover = mover;
		_rotator = rotator;
		_spawnTimer = spawnTimer;
		_timeToSpawn = timeToSpawn;

		_spawnTimer.StartProcess(_timeToSpawn);

		foreach (IInitializable initializable in GetComponentsInChildren<IInitializable>())
			initializable.Initialize();
	}

	private void Update()
	{
		_rotator.Update(Time.deltaTime);
	}

	public void SetDestination(Vector3 position) => _mover.SetDestination(position);

	public void StopMove() => _mover.Stop();

	public void ResumeMove() => _mover.Resume();

	public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

	public bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget)
		=> NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);

	public bool IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData)
	{
		if (_agent.isOnOffMeshLink)
		{
			offMeshLinkData = _agent.currentOffMeshLinkData;
			return true;
		}

		offMeshLinkData = default(OffMeshLinkData);
		return false;
	}


	public bool InSpawnProcess(out float elapsedTime) => _spawnTimer.InProcess(out elapsedTime);

	public void Kill()
	{
		Debug.Log("Враг убит");

		Destroy();
	}
}
