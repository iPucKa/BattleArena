using System;
using System.Collections.Generic;

public class RuleService
{
	public event Action IsDone;

	private Dictionary<GameRules, Func<bool>> _myRules = new Dictionary<GameRules, Func<bool>>();
		
	public void AddTo(GameRules rule, Func<bool> condition)
	{
		if(_myRules.ContainsKey(rule) == false)		
			_myRules.Add(rule, condition);
	}

	public void Update(float deltatime)
	{
		foreach (KeyValuePair<GameRules, Func<bool>> gameRule in _myRules)
		{
			if (gameRule.Value.Invoke())
			{
				IsDone?.Invoke();
				break;
			}
		}
	}
}
