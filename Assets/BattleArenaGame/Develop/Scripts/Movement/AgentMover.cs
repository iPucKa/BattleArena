using UnityEngine;
using UnityEngine.AI;

public class AgentMover
{
	private NavMeshAgent _agent;

	public Vector3 CurrentVelocity => _agent.desiredVelocity;

	public AgentMover(NavMeshAgent agent, float movementSpeed)
	{
		_agent = agent;
		_agent.speed = movementSpeed;
		_agent.acceleration = 999;
	}

	public void SetDestination(Vector3 position)
	{
		_agent.SetDestination(position);
	}

	public void Stop()
	{
		_agent.isStopped = true;
	}

	public void Resume()
	{
		_agent.isStopped = false;
	}
}
