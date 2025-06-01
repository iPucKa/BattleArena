using UnityEngine;

public abstract class DirectionalMover
{
	private float _movementSpeed;

	private Vector3 _currentDirection;

	public DirectionalMover(float movementSpeed)
	{
		_movementSpeed = movementSpeed;
	}

	public Vector3 CurrentVelocity => _currentDirection.normalized * _movementSpeed;

	public void SetInputDirection(Vector3 direction) => _currentDirection = direction;

	public abstract void Update(float deltaTime);
}
