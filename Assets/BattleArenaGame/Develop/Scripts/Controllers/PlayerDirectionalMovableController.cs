using UnityEngine;

public class PlayerDirectionalMovableController : Controller
{
	private IDirectionalMovable _movable;

	public PlayerDirectionalMovableController(IDirectionalMovable movable)
	{
		_movable = movable;
	}

	protected override void UpdateLogic(float deltaTime)
	{
		Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		_movable.SetMoveDirection(inputDirection);
	}
}
