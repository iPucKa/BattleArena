using System;
using System.Collections.Generic;

public class ItemList<TItem> 
{
    private List<TItem> _enemies = new();

	public int Count => _enemies.Count;

	public TItem GetBy(int index) => _enemies[index];

	public void Add(TItem enemy)
    {
		if (_enemies.Contains(enemy))
		{
			throw new InvalidOperationException($"{nameof(enemy)} is already exist");			
		}

		_enemies.Add(enemy);
	}

	public void Remove(TItem enemy)
	{
		if (_enemies.Contains(enemy) == false)
		{
			throw new InvalidOperationException($"{nameof(enemy)} is already exist");			
		}

		_enemies.Remove(enemy);
	}

	public void Clear() => _enemies.Clear();

	//public List<TItem> Items => _enemies;
}
