using System;
using UnityEngine;

public class MovableView : MonoBehaviour, IInitializable
{
	private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
	private readonly int AttackedKey = Animator.StringToHash("Attacked");
	private readonly int KilledKey = Animator.StringToHash("Killed");

	[SerializeField] private Animator _animator;

	private IMovable _movable;
	private IDamageable _damagable;

	private bool _isInit;

	public void Initialize()
	{
		_movable = GetComponentInParent<IMovable>();
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

		if (_movable.CurrentVelocity.magnitude > 0.05f)
			StartRunning();
		else
			StopRunning();
	}

	private void StopRunning()
	{
		_animator.SetBool(IsRunningKey, false);
	}

	private void StartRunning()
	{
		_animator.SetBool(IsRunningKey, true);
	}
}
