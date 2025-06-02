using UnityEngine;

public class DamagableView : MonoBehaviour, IInitializable
{
	private readonly int KilledKey = Animator.StringToHash("Killed");

	[SerializeField] private Animator _animator;

	private IKillable _damagable;

	private bool _isInit;

	public void Initialize()
	{
		_damagable = GetComponentInParent<IKillable>();

		_damagable.Killed += OnKilled;
		_isInit = true;
	}

	private void OnKilled()
	{
		_animator.SetTrigger(KilledKey);
	}

	private void Update()
	{
		if (_isInit == false)
			return;
	}	
}
