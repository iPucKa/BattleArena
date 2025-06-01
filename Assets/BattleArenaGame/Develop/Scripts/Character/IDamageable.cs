using System;

public interface IDamageable
{
	event Action Killed;
	void TakeDamage();
}
