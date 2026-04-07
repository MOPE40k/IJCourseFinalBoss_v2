using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Levels/NewLevelConfig", fileName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Settings:")]
        [SerializeField] private List<StageConfig> _stageConfigs;
        [SerializeField] private float _heroHealth = 100f;
        [SerializeField] private float _preparationStateTime = 15f;

        // Runtime
        public IReadOnlyList<StageConfig> StageConfigs => _stageConfigs;
        public float HeroHealth => _heroHealth;
        public float PreparationStateTime => _preparationStateTime;
    }
}
