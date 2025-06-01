using Cinemachine;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class MainHeroFactory : IDisposable
{
	private ControllersUpdateService _controllersUpdateService;
	private ControllersFactory _controllersFactory;
	private CharacterFactory _charactersFactory;

	private Controller _controller;

	public MainHeroFactory(
		ControllersUpdateService controllersUpdateService,
		ControllersFactory controllersFactory,
		CharacterFactory charactersFactory)
	{
		_controllersUpdateService = controllersUpdateService;
		_controllersFactory = controllersFactory;
		_charactersFactory = charactersFactory;
	}

	public Character Create(MainHeroConfig config, Vector3 spawnPosition)
	{
		Character instance = _charactersFactory.CreateCharacter(
			config.Prefab,
			spawnPosition,
			config.MoveSpeed,
			config.RotationSpeed);

		instance.Killed += OnMainHeroKilled;

		CinemachineVirtualCamera followCameraPrefab = Resources.Load<CinemachineVirtualCamera>("FollowCamera");

		CinemachineVirtualCamera followCamera = Object.Instantiate(followCameraPrefab);

		followCamera.Follow = instance.CameraTarget;

		_controller = _controllersFactory.CreateMainHeroPlayerController(instance);

		_controller.Enable();

		_controllersUpdateService.Add(_controller, () => instance.IsDestroyed);

		return instance;
	}

	public void Dispose()
	{
		//
	}

	private void OnMainHeroKilled()
	{
		//_controller.Disable();
	}
}
