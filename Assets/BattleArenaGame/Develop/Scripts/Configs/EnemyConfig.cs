using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Gameplay/EnemyConfig", fileName = "EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
	[field: SerializeField] public Enemy Prefab { get; private set; }
	[field: SerializeField] public float MoveSpeed { get; private set; } = 6;
	[field: SerializeField] public float RotationSpeed { get; private set; } = 900;
	//[field: SerializeField] public float JumpSpeed { get; private set; } = 5;
	//[field: SerializeField] public AnimationCurve JumpCurve { get; private set; }
	[field: SerializeField] public float TimeToSpawn { get; private set; } = 1;
	//[field: SerializeField] public float AgroRange { get; private set; } = 30;
	//[field: SerializeField] public float TimeForIdle { get; private set; } = 1;
	//[field: SerializeField] public float MinDistanceToTarget { get; private set; } = 1;
}
