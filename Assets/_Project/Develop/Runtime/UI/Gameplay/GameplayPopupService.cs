using System;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPopupService : PopupService
    {
        // References
        private readonly GameplayUiRoot _gameplayUiRoot = null;
        private readonly GameplayPresentersFactory _gameplayPresentersFactory = null;

        public GameplayPopupService(
            GameplayUiRoot gameplayUiRoot,
            GameplayPresentersFactory gameplayPresentersFactory,
            ViewsFactory viewsFactory,
            ProjectPresentersFactory projectPresentersFactory) : base(viewsFactory, projectPresentersFactory)
        {
            _gameplayUiRoot = gameplayUiRoot;
            _gameplayPresentersFactory = gameplayPresentersFactory;
        }

        protected override Transform PopupLayer => _gameplayUiRoot.PopupsLayer;

        public WinPopupPresenter OpenWinPopup(Action closeCallback = null)
        {
            WinPopupView view = ViewsFactory
                .Create<WinPopupView>(ViewIDs.WinPopup, PopupLayer);

            WinPopupPresenter presenter = _gameplayPresentersFactory
                .CreateWinPopupPresenter(view);

            OnPopupCreated(presenter, view, closeCallback);

            return presenter;
        }

        public DefeatPopupPresenter OpenDefeatPopup(Action closeCallback = null)
        {
            DefeatPopupView view = ViewsFactory
                .Create<DefeatPopupView>(ViewIDs.DefeatPopup, PopupLayer);

            DefeatPopupPresenter presenter = _gameplayPresentersFactory
                .CreateDefeatPopupPresenter(view);

            OnPopupCreated(presenter, view, closeCallback);

            return presenter;
        }
    }
}