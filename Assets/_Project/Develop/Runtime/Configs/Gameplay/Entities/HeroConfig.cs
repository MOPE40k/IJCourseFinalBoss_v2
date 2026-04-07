using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/NewHeroConfig", fileName = "HeroConfig")]
    public class HeroConfig : EntityConfig
    {
        [Header("Prefab Settings:")]
        [SerializeField] private string _prefabPath = "Entities/Hero";

        [Space]
        [Header("Movement Settings:")]
        [SerializeField, Min(0)] private float _moveSpeed = 9;
        [SerializeField, Min(0)] private float _rotationSpeed = 900;

        [Space]
        [Header("Attack Settings:")]
        [SerializeField, Min(0)] private float _attackProcessTime = 1f;
        [SerializeField, Min(0)] private float _attackDelayTime = 0.5f;
        [SerializeField, Min(0)] private float _attackCooldown = 0.5f;
        [SerializeField, Min(0)] private float _radiusAreaAttack = 5f;
        [SerializeField, Min(0)] private int _costPerMine = 100;

        [Space]
        [Header("Damage Settings:")]
        [SerializeField, Min(0)] private float _instantAttackDamage = 50;
        [SerializeField, Min(0)] private float _damageToArea = 25f;

        [Space]
        [Header("Lifecycle Settings:")]
        [SerializeField, Min(0)] private float _maxHealth = 100;
        [SerializeField, Min(0)] private float _deathProcessTime = 2;

        // Runtime
        public string PrefabPath => _prefabPath;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float AttackProcessTime => _attackProcessTime;
        public float AttackDelayTime => _attackDelayTime;
        public float AttackCooldown => _attackCooldown;
        public float InstantAttackDamage => _instantAttackDamage;
        public float DamageToArea => _damageToArea;
        public float RadiusAreaAttack => _radiusAreaAttack;
        public int CostPerMine => _costPerMine;
        public float MaxHealth => _maxHealth;
        public float DeathProcessTime => _deathProcessTime;
    }
}
