using System;
using UnityEngine;

public class Character : MonoDestroyable, IDirectionalMovable, IDirectionalRotatable, IKillable, ICanShoot
{
	public event Action Killed;

	private DirectionalMover _mover;
	private DirectionalRotator _rotator;

	[SerializeField] private Transform _cameraTarget;
	[SerializeField] private Transform _bulletSpawnPoint;
	[SerializeField] private Bullet _bulletPrefab;

	public Vector3 CurrentVelocity => _mover.CurrentVelocity;

	public Quaternion CurrentRotation => _rotator.CurrentRotation;

	public Vector3 Position => transform.position;

	public Transform CameraTarget => _cameraTarget;	

	public void Initialize(DirectionalMover mover, DirectionalRotator rotator)
	{
		_mover = mover;
		_rotator = rotator;

		foreach (IInitializable initializable in GetComponentsInChildren<IInitializable>())
			initializable.Initialize();
	}

	private void Update()
	{
		_mover.Update(Time.deltaTime);
		_rotator.Update(Time.deltaTime);		
	}

	public void SetMoveDirection(Vector3 inputDirection) => _mover.SetInputDirection(inputDirection);

	public void SetRotationDirection(Vector3 inputDirection) => _rotator.SetInputDirection(inputDirection);

	public void Kill()
	{
		Debug.Log("Игрок убит");

		Killed?.Invoke();
	}

	public void Shoot(out Bullet bullet)
	{
		Vector3 position = _bulletSpawnPoint.position;		

		Quaternion rotation = _bulletSpawnPoint.rotation;

		bullet = Instantiate(_bulletPrefab, position, rotation);
	}
}
