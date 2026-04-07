using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Stages
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Stages/NewClearAllEnemiesStage", fileName = "ClearAllEnemiesStage")]
    public class ClearAllEnemiesStageConfig : StageConfig
    {
        [Header("Enemies:")]
        [SerializeField] private List<EnemyItemConfig> _enemyItems;

        [Space]
        [Header("Spawn Settings:")]
        [SerializeField] private float _minRadius = 15f;
        [SerializeField] private float _maxRadius = 30f;

        [Space]
        [Header("Reward Settings:")]
        [SerializeField, Min(0)] public int _rewardPerEnemy = 50;


        // Runtime
        public IReadOnlyList<EnemyItemConfig> EnemyItems => _enemyItems;
        public float MinRadius => _minRadius;
        public float MaxRadius => _maxRadius;
        public int RewardPerEnemy => _rewardPerEnemy;
    }
}
