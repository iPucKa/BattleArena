public class AlongMovableVelocityRotatableController : Controller
{
	private IDirectionalRotatable _rotatable;
	private IDirectionalMovable _movable;

	public AlongMovableVelocityRotatableController(IDirectionalRotatable rotatable, IDirectionalMovable movable)
	{
		_rotatable = rotatable;
		_movable = movable;
	}

	protected override void UpdateLogic(float deltaTime)
	{
		_rotatable.SetRotationDirection(_movable.CurrentVelocity);
	}
}
