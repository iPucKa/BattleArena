using UnityEngine;

public abstract class DirectionalRotator
{
	private float _rotationSpeed;

	private Vector3 _currentDirection;

	public DirectionalRotator(float rotationSpeed)
	{
		_rotationSpeed = rotationSpeed;
	}

	public abstract Quaternion CurrentRotation { get; }

	public void SetInputDirection(Vector3 direction) => _currentDirection = direction;

	public void Update(float deltaTime)
	{
		if (_currentDirection.magnitude < 0.05f)
			return;

		Quaternion lookRotation = Quaternion.LookRotation(_currentDirection.normalized);

		float step = _rotationSpeed * deltaTime;

		ApplyRotation(Quaternion.RotateTowards(CurrentRotation, lookRotation, step));
	}

	protected abstract void ApplyRotation(Quaternion rotation);
}
