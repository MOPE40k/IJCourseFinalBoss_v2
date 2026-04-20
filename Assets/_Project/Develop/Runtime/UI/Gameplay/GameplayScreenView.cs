using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        [Header("References:")]
        [SerializeField] private IconTextListView _walletView = null;
        [SerializeField] private IconTextView _stageNumberView = null;
        [SerializeField] private EnititiesHealthDisplay _entitiesHealthDisplay = null;

        // Runtime
        public IconTextListView WalletView => _walletView;
        public IconTextView StageNumberView => _stageNumberView;
        public EnititiesHealthDisplay EntityHealthPresenter => _entitiesHealthDisplay;
    }
}