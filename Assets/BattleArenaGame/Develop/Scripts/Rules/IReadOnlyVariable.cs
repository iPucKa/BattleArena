using System;

public interface IReadOnlyVariable<TValue>
{
	event Action<TValue> ValueChanged;

	TValue Value { get; }
}
