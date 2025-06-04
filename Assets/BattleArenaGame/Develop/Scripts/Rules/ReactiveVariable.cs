using System;

public class ReactiveVariable<TValue> : IReadOnlyVariable<TValue> where TValue : IEquatable<TValue>
{
	public event Action<TValue> ValueChanged;

	private TValue _value;

	public ReactiveVariable()
	{
		_value = default(TValue);
	}

	public ReactiveVariable(TValue value)
	{
		_value = value;
	}

	public TValue Value
	{
		get => _value;
		set
		{
			TValue oldValue = _value;
			_value = value;

			if (_value.Equals(oldValue) == false)
				ValueChanged?.Invoke(_value);
		}
	}
}
