using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups
{
    public class DefeatPopupPresenter : PopupPresenterBase
    {
        // Consts
        private const string TitleText = "You defeat!";

        private readonly DefeatPopupView _view = null;
        private readonly SceneSwitcherService _sceneSwitcherService = null;
        private readonly GameplayInputArgs _currentLevelArgs = null;
        private readonly ICoroutinesPerformer _coroutinesPerformer = null;

        public DefeatPopupPresenter(
            DefeatPopupView view,
            SceneSwitcherService sceneSwitcherService,
            GameplayInputArgs gameplayInputArgs,
            ICoroutinesPerformer coroutinesPerformer) : base(coroutinesPerformer)
        {
            _view = view;
            _sceneSwitcherService = sceneSwitcherService;
            _currentLevelArgs = gameplayInputArgs;
            _coroutinesPerformer = coroutinesPerformer;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle(TitleText);

            _view.ContinueClicked += OnContinueClicked;
            _view.RestartClicked += OnRestartClicked;
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.ContinueClicked -= OnContinueClicked;
            _view.RestartClicked -= OnRestartClicked;
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            _view.ContinueClicked -= OnContinueClicked;
            _view.RestartClicked -= OnRestartClicked;
        }

        private void OnContinueClicked()
        {
            _coroutinesPerformer.StartPerform(
                _sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));

            OnCloseRequest();
        }

        private void OnRestartClicked()
        {
            _coroutinesPerformer.StartPerform(
                _sceneSwitcherService.ProcessSwitchTo(
                    Scenes.Gameplay,
                    new GameplayInputArgs(_currentLevelArgs.LevelNumber)));

            OnCloseRequest();
        }
    }
}