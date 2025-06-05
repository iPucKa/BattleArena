using System;
using System.Collections.Generic;

public class MyCollection<TItem> where TItem : MonoDestroyable
{
	private List<TItem> _enemies = new();

	private ReactiveVariable<int> _currentValue;
	private ReactiveVariable<int> _maxValue;

	public MyCollection(ReactiveVariable<int> currentValue, ReactiveVariable<int> maxValue)
	{
		_currentValue = currentValue;
		_maxValue = maxValue;
	}

	public MyCollection()
	{
		_currentValue = new ReactiveVariable<int>();
		_maxValue = new ReactiveVariable<int>();
	}

	public int Count => _currentValue.Value;

	public TItem GetBy(int index) => _enemies[index];

	public void Add(TItem item)
    {
		if (_enemies.Contains(item))
		{
			throw new InvalidOperationException($"{nameof(item)} is already exist");			
		}

		_enemies.Add(item);

		_maxValue.Value++;
		_currentValue.Value = _enemies.Count;		
	}

	public void Remove(TItem item)
	{
		if (_enemies.Contains(item) == false)
		{
			throw new InvalidOperationException($"{nameof(item)} is already exist");			
		}

		_enemies.Remove(item);
		_currentValue.Value = _enemies.Count;
	}

	public void Clear()
	{
		_enemies.Clear();

		_currentValue.Value = 0;
		_maxValue.Value = 0;
	}
}
