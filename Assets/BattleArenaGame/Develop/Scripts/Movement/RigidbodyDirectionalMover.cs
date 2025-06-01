using UnityEngine;

public class RigidbodyDirectionalMover : DirectionalMover
{
	private Rigidbody _rigidbody;

	public RigidbodyDirectionalMover(Rigidbody rigidbody, float movementSpeed) : base(movementSpeed)
	{
		_rigidbody = rigidbody;
	}

	public override void Update(float deltaTime) => _rigidbody.velocity = CurrentVelocity;
}
