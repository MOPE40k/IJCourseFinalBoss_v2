using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Project.Develop.Runtime.Meta.Features.Sessions;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Meta.SessionResult
{
    [CreateAssetMenu(menuName = "Configs/Meta/SessionsResults/New SessionResultIconConfig", fileName = "SessionResultIconConfig")]
    public class SessionResultIconConfig : ScriptableObject
    {
        [Header("Sessions Results Icons Settings:")]
        [SerializeField] private List<IconsConfig> _configs = null;

        public Sprite GetSpriteFor(SessionEndConditionTypes type)
            => _configs.First(config => config.Type == type).Sprite;

        [Serializable]
        private class IconsConfig
        {
            [field: SerializeField] public SessionEndConditionTypes Type { get; private set; } = SessionEndConditionTypes.Win;
            [field: SerializeField] public Sprite Sprite { get; private set; } = null;
        }
    }
}