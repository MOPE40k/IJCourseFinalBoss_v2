using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using Assets._Project.Develop.Runtime.UI.Wallet;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        // References
        private readonly GameplayScreenView _view = null;
        private readonly ProjectPresentersFactory _projectPresentersFactory = null;
        private readonly GameplayPresentersFactory _gameplayPresentersFactory = null;

        // Runtime
        private readonly List<IPresenter> _childPresenters = new();
        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter = null;

        public GameplayScreenPresenter(
            GameplayScreenView view,
            ProjectPresentersFactory projectPresentersFactory,
            GameplayPresentersFactory gameplayPresentersFactory)
        {
            _view = view;
            _projectPresentersFactory = projectPresentersFactory;
            _gameplayPresentersFactory = gameplayPresentersFactory;
        }

        public void Initialize()
        {
            CreateWallet();

            CreateStageNumber();

            CreateEntitiesHealthDisplayPresenter();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void LateUpdateTick()
            => _entitiesHealthDisplayPresenter.LateUpdateTick();

        public void Dispose()
        {
            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_view.WalletView);

            _childPresenters.Add(walletPresenter);
        }

        private void CreateStageNumber()
        {
            StagePresenter stagePresenter = _gameplayPresentersFactory.CreateStagePresenter(_view.StageNumberView);

            _childPresenters.Add(stagePresenter);
        }

        private void CreateEntitiesHealthDisplayPresenter()
        {
            _entitiesHealthDisplayPresenter = _gameplayPresentersFactory
                .CreateEntitiesHealthDisplayPresenter(_view.EntityHealthPresenter);

            _childPresenters.Add(_entitiesHealthDisplayPresenter);
        }
    }
}