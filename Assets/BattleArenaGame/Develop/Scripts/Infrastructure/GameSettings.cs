using System;
using UnityEngine;

[Serializable]
public class GameSettings
{ 
	[SerializeField] private GameRules _rule;

	public GameRules Rule => _rule;
}
