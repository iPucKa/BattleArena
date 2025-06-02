using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField] private Collider _collider;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out Enemy damageable))
			if(damageable.InSpawnProcess(out float elapsedTime)==false)
				damageable.Kill();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.TryGetComponent(out Character character))
			character.Kill();
	}
}
