using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GamePlay/MainHeroConfig", fileName = "MainHeroConfig")]
public class MainHeroConfig : ScriptableObject
{
    [field: SerializeField] public Character Prefab {  get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 9;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 900;
}
