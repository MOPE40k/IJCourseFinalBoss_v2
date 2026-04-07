using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/NewGhostConfig", fileName = "GhostConfig")]
    public class GhostConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Ghost";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 9f;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900f;
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 100f;
        [field: SerializeField, Min(0)] public float BodyContactDamage { get; private set; } = 50f;
        [field: SerializeField, Min(0)] public float DamageAreaAttack { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public float RadiusAreaAttack { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 2f;
    }
}
