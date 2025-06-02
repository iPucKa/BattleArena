using System;

public interface IKillable
{
	event Action Killed;
	void Kill();
}
