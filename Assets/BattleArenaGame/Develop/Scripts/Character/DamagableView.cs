using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableView : MonoBehaviour, IInitializable
{
	private readonly int AttackedKey = Animator.StringToHash("Attacked");
	private readonly int KilledKey = Animator.StringToHash("Killed");

	[SerializeField] private Animator _animator;

	private IDamageable _damagable;

	private bool _isInit;

	public void Initialize()
	{
		_damagable = GetComponentInParent<IDamageable>();

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
