using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        // Delegates
        public event Action OpenLevelsMenuButtonClicked = null;

        [Header("References:")]
        [SerializeField] private IconTextListView _walletView = null;
        [SerializeField] private IconTextListView _sessionsResultsView = null;
        [SerializeField] private Button _openLevelsMenuButton = null;

        // Runtime
        public IconTextListView WalletView => _walletView;
        public IconTextListView SessionsResultsView => _sessionsResultsView;

        private void OnEnable()
            => _openLevelsMenuButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);

        private void OnDisable()
            => _openLevelsMenuButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);

        private void OnOpenLevelsMenuButtonClicked()
            => OpenLevelsMenuButtonClicked?.Invoke();
    }
}
