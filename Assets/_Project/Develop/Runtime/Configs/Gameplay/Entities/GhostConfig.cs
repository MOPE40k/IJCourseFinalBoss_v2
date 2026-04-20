using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/NewGhostConfig", fileName = "GhostConfig")]
    public class GhostConfig : EntityConfig
    {
        [Header("Prefab Settings:")]
        [SerializeField] public string _prefabPath = "Entities/Ghost";

        [Space]
        [Header("Moving Settings:")]
        [SerializeField, Min(0)] public float _moveSpeed = 9f;
        [SerializeField, Min(0)] public float _rotationSpeed = 900f;

        [Space]
        [Header("Attack Settings:")]
        [SerializeField, Min(0)] public float _attackProcessTime = 1f;
        [SerializeField, Min(0)] public float _attackDelayTime = 0.5f;
        [SerializeField, Min(0)] public float _attackCooldown = 0.5f;

        [Space]
        [Header("Damage Settings:")]
        [SerializeField, Min(0)] public float _bodyContactDamage = 50f;
        [SerializeField, Min(0)] public float _damageAreaAttack = 15f;
        [SerializeField, Min(0)] public float _radiusAreaAttack = 5f;

        [Space]
        [Header("Lifecycle Settings:")]
        [SerializeField, Min(0)] public float _maxHealth = 100f;
        [SerializeField, Min(0)] public float _deathProcessTime = 2f;

        [Space]
        [Header("Spawn Settings:")]
        [SerializeField] private float _spawnProcessTime = 2f;

        // Runtime
        public string PrefabPath => _prefabPath;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float AttackProcessTime => _attackProcessTime;
        public float AttackDelayTime => _attackDelayTime;
        public float AttackCooldown => _attackCooldown;
        public float BodyContactDamage => _bodyContactDamage;
        public float DamageAreaAttack => _damageAreaAttack;
        public float RadiusAreaAttack => _radiusAreaAttack;
        public float MaxHealth => _maxHealth;
        public float DeathProcessTime => _deathProcessTime;
        public float SpawnProcessTime => _spawnProcessTime;
    }
}
