using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Gameplay/LevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ScriptableObject
{
	//[field: SerializeField] public float DistanceTraveledToWin { get; private set; }
	//[field: SerializeField] public float TimeToDefeat { get; private set; }

	[field: SerializeField] public List<GameSettings> WinSettings { get; private set; }
	[field: SerializeField] public List<GameSettings> DefeatSettings { get; private set; }

	[field: SerializeField] public EnemyConfig EnemyConfig { get; private set; }
	[field: SerializeField] public int EnemiesCount { get; private set; }
	[field: SerializeField] public float CooldownSpawnTime { get; private set; }

	//[field: SerializeField] public float EnemiesSpawnRange { get; private set; }
	[field: SerializeField] public List<Vector3> EnemiesSpawnPoints { get; private set; }
	[field: SerializeField] public Vector3 MainHeroStartPosition { get; private set; }
	[field: SerializeField] public string EnvironmentSceneName { get; private set; }

	[ContextMenu("UpdateStartHeroPosition")]
	private void UpdateStartHeroPosition()
	{
		GameObject point = GameObject.FindGameObjectWithTag("StartHeroPosition");
		MainHeroStartPosition = point.transform.position;
	}

	[ContextMenu("UpdateSpawnEnemyPosition")]
	private void UpdateSpawnEnemyPosition()
	{
		GameObject[] points = GameObject.FindGameObjectsWithTag("SpawnEnemyPosition");
		
		List<Vector3> EnemiesSpawnPositions = new List<Vector3>();

		for (int i = 0; i < points.Length; i++)		
			EnemiesSpawnPositions.Add(points[i].transform.position);

		EnemiesSpawnPoints = EnemiesSpawnPositions;
	}
}
