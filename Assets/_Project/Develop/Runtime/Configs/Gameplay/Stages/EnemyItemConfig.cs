using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Stages
{
    [Serializable]
    public class EnemyItemConfig
    {
        [Space]
        [Header("References:")]
        [SerializeField] public EntityConfig _enemyConfig = null;

        public EntityConfig EnemyConfig => _enemyConfig;
    }
}
