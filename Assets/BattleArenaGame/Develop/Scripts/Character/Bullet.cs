using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoDestroyable
{
	[SerializeField] private float _speed;

	private Rigidbody _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();

		_rigidbody.AddRelativeForce(Vector3.up * _speed, ForceMode.Impulse);
	}	
}
