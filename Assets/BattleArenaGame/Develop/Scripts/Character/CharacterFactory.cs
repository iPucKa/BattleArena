using System;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class CharacterFactory
{
	public Character CreateCharacter(
		Character prefab,
		Vector3 spawnPosition,
		float moveSpeed,
		float rotationSpeed)
	{
		Character instance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, null);

		DirectionalMover mover;

		DirectionalRotator rotator;

		if (instance.TryGetComponent(out CharacterController characterController))
		{
			mover = new CharacterControllerDirectionalMover(characterController, moveSpeed);
			rotator = new TransformDirectionalRotator(instance.transform, rotationSpeed);
		}
		else if (instance.TryGetComponent(out Rigidbody rigidbody))
		{
			mover = new RigidbodyDirectionalMover(rigidbody, moveSpeed);
			rotator = new RigidbodyDirectionalRotator(rigidbody, rotationSpeed);
		}
		else
		{
			throw new InvalidOperationException("Not found mover component");
		}

		instance.Initialize(mover, rotator);

		return instance;
	}

	public Enemy CreateEnemy(
		Enemy prefab,
		Vector3 spawnPosition,
		float moveSpeed,
		float rotationSpeed,
		//float jumpSpeed,
		//AnimationCurve jumpCurve,
		float timeToSpawn)
	{
		Enemy instance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, null);

		if (instance.TryGetComponent(out NavMeshAgent agent) == false)
			throw new InvalidOperationException("Not found agent component");

		agent.updateRotation = false;

		AgentMover mover = new AgentMover(agent, moveSpeed);
		TransformDirectionalRotator rotator = new TransformDirectionalRotator(instance.transform, rotationSpeed);		

		Timer spawnTimer = new Timer(instance);

		instance.Initialize(
			agent,
			mover,
			rotator,
			spawnTimer,
			timeToSpawn);

		return instance;
	}
}
