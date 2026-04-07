using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.SessionsResults;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _screen = null;

        private readonly ProjectPresentersFactory _projectPresentersFactory = null;

        private readonly SceneSwitcherService _sceneSwitcherService = null;
        private readonly ICoroutinesPerformer _coroutinesPerformer = null;
        private readonly ConfigsProviderService _configsProviderService = null;

        //private readonly MainMenuPopupService _popupService;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            ConfigsProviderService configsProviderService)
        //MainMenuPopupService popupService)
        {
            _screen = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _configsProviderService = configsProviderService;
            //_popupService = popupService;
        }

        public void Initialize()
        {
            _screen.OpenLevelsMenuButtonClicked += OnOpenLevelsMenuButtonClicked;

            CreateWallet();

            CreateSessionsResults();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _screen.OpenLevelsMenuButtonClicked -= OnOpenLevelsMenuButtonClicked;

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);

            _childPresenters.Add(walletPresenter);
        }

        private void CreateSessionsResults()
        {
            SessionsResultsPresenter sessionsResultsPresenter = _projectPresentersFactory
                .CreateSessionsResultsPresenter(_screen.SessionsResultsView);

            _childPresenters.Add(sessionsResultsPresenter);
        }

        private void OnOpenLevelsMenuButtonClicked()
        {
            //_popupService.OpenLevelsMenuPopup();

            SwitchToRandomLevel();
        }

        private void SwitchToRandomLevel()
        {
            int levelsCount = _configsProviderService.GetConfig<LevelsListConfig>().Levels.Count;

            int randomLevel = Random.Range(1, levelsCount);

            _coroutinesPerformer
                .StartPerform(_sceneSwitcherService.ProcessSwitchTo(
                    Scenes.Gameplay,
                    new GameplayInputArgs(randomLevel)));
        }
    }
}
