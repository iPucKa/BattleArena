using UnityEngine;

public interface IDirectionalMovable : ITransformPosition, IMovable
{
	void SetMoveDirection(Vector3 inputDirection);
}
