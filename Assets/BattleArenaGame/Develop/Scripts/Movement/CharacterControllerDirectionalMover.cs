using UnityEngine;

public class CharacterControllerDirectionalMover : DirectionalMover
{
	private CharacterController _characterController;

	public CharacterControllerDirectionalMover(CharacterController characterController, float movementSpeed) : base(movementSpeed)
	{
		_characterController = characterController;
	}

	public override void Update(float deltaTime) => _characterController.Move(CurrentVelocity * deltaTime);
}
