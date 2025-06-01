using System;
using UnityEngine;

public class EnemiesFactory : IDisposable
{
	private ControllersUpdateService _controllersUpdateService;
	private ControllersFactory _controllersFactory;
	private CharacterFactory _charactersFactory;

	private Controller _controller;

	public EnemiesFactory(
		ControllersUpdateService controllersUpdateService,
		ControllersFactory controllersFactory,
		CharacterFactory charactersFactory)
	{
		_controllersUpdateService = controllersUpdateService;
		_controllersFactory = controllersFactory;
		_charactersFactory = charactersFactory;
	}

	public Enemy CreateAgentEnemy(EnemyConfig config, Vector3 spawnPosition)
	{
		Enemy instance = _charactersFactory.CreateEnemy(
				config.Prefab,
				spawnPosition,
				config.MoveSpeed,
				config.RotationSpeed,
				//config.JumpSpeed,
				//config.JumpCurve,
				config.TimeToSpawn);

		instance.Killed += OnEnemyKilled;

		_controller = _controllersFactory.CreateEnemyIdleController(instance);

		_controller.Enable();

		_controllersUpdateService.Add(_controller, () => instance.IsDestroyed);

		return instance;
	}

	public void Dispose()
	{
		//
	}

	private void OnEnemyKilled()
	{
		//_controller.Disable();
	}
}
