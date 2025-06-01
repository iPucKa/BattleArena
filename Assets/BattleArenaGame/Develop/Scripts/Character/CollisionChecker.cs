using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private Collider _collider;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IDamageable damageable))
			damageable.TakeDamage();		
	}
}
