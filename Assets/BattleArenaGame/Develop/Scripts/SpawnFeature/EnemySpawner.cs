using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner
{
	//public event Action EnemyKilled;

	private EnemiesFactory _enemiesFactory;
	private List<Enemy> _spawnedEnemies = new List<Enemy>();

	public EnemySpawner(EnemiesFactory enemiesFactory)
	{
		_enemiesFactory = enemiesFactory;
	}

	//public List<Enemy> Spawn(EnemyConfig enemyConfig, Vector3 spawnPosition)
	//{
	//	NavMeshQueryFilter queryFilter = new NavMeshQueryFilter();
	//	queryFilter.agentTypeID = 0;
	//	queryFilter.areaMask = 1;

	//	if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit spawnPoint, 0.1f, queryFilter))
	//	{
	//		Enemy enemy = _enemiesFactory.CreateAgentEnemy(enemyConfig, spawnPoint.position);
	//		_spawnedEnemies.Add(enemy);

	//		enemy.Killed += OnEnemyKilled;
	//	}

	//	return _spawnedEnemies;
	//}

	public Enemy Spawn(EnemyConfig enemyConfig, Vector3 spawnPosition)
	{
		NavMeshQueryFilter queryFilter = new NavMeshQueryFilter();
		queryFilter.agentTypeID = 0;
		queryFilter.areaMask = 1;

		if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit spawnPoint, 0.5f, queryFilter))
		{
			return _enemiesFactory.CreateAgentEnemy(enemyConfig, spawnPoint.position);

			//enemy.Killed += OnEnemyKilled;

			//return enemy;
		}

		return null;
	}

	//private void OnEnemyKilled()
	//{
	//	EnemyKilled?.Invoke();
	//}
}
