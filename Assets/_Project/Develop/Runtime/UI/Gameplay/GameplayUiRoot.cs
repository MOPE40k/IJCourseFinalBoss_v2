using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayUiRoot : MonoBehaviour
    {
        [Header("References:")]
        [SerializeField] private Transform _hudLayer = null;
        [SerializeField] private Transform _popupsLayer = null;
        [SerializeField] private Transform _vfxUnderPopupsLayer = null;
        [SerializeField] private Transform _vfxOverPopupsLayer = null;

        // Runtime
        public Transform HudLayer => _hudLayer;
        public Transform PopupsLayer => _popupsLayer;
        public Transform VfxUnderPopupsLayer => _vfxUnderPopupsLayer;
        public Transform VfxOverPopupsLayer => _vfxOverPopupsLayer;
    }
}